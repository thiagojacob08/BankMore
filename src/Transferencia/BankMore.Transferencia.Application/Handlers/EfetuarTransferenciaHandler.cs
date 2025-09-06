using BankMore.Transferencia.Application.Commands;
using BankMore.Transferencia.Application.Services;
using BankMore.Transferencia.Domain.Entities;
using BankMore.Transferencia.Domain.Interfaces;
using MediatR;

public class EfetuarTransferenciaHandler : IRequestHandler<EfetuarTransferenciaCommand>
{
    private readonly ITransferenciaRepository _transferenciaRepository;
    private readonly IIdempotenciaRepository _idempotenciaRepository;
    private readonly IContaCorrenteApiService _contaService;
    private readonly ITransferenciaEventProducer _eventProducer;


    public EfetuarTransferenciaHandler(
        ITransferenciaRepository transferenciaRepository,
        IIdempotenciaRepository idempotenciaRepository,
        IContaCorrenteApiService contaService,
        ITransferenciaEventProducer eventProducer)
    {
        _transferenciaRepository = transferenciaRepository;
        _idempotenciaRepository = idempotenciaRepository;
        _contaService = contaService;
        _eventProducer = eventProducer;
    }

    public async Task<Unit> Handle(EfetuarTransferenciaCommand request, CancellationToken cancellationToken)
    {
        // ---------------- Idempotência ----------------
        var existente = await _idempotenciaRepository.GetByChaveAsync(request.ChaveIdempotencia);
        if (existente != null)
            return Unit.Value;

        // ---------------- Validações ----------------
        if (request.Valor <= 0)
            throw new InvalidOperationException("Valor deve ser positivo.");
        if (string.IsNullOrEmpty(request.IdContaOrigem) || string.IsNullOrEmpty(request.IdContaDestino))
            throw new InvalidOperationException("Contas inválidas.");

        // ---------------- Débito e Crédito ----------------
        await _contaService.MovimentarDebitoAsync(request.IdContaOrigem, request.Valor, request.ChaveIdempotencia);

        try
        {
            await _contaService.MovimentarCreditoAsync(request.IdContaDestino, request.Valor, request.ChaveIdempotencia);
        }
        catch
        {
            // Rollback
            await _contaService.MovimentarCreditoAsync(request.IdContaOrigem, request.Valor, Guid.NewGuid().ToString());
            throw;
        }

        // ---------------- Persistir Transferencia ----------------
        var transferencia = new Transferencia
        {
            IdTransferencia = Guid.NewGuid().ToString(),
            IdContaOrigem = request.IdContaOrigem,
            IdContaDestino = request.IdContaDestino,
            Valor = request.Valor,
            DataMovimento = DateTime.UtcNow
        };
        await _transferenciaRepository.AddAsync(transferencia);

        // ---------------- Registrar Idempotência ----------------
        var idempotencia = new Idempotencia
        {
            ChaveIdempotencia = request.ChaveIdempotencia,
            Requisicao = $"Origem:{request.IdContaOrigem},Destino:{request.IdContaDestino},Valor:{request.Valor}",
            Resultado = "Transferência realizada"
        };
        await _idempotenciaRepository.AddAsync(idempotencia);

        await _transferenciaRepository.SaveChangesAsync();
        await _idempotenciaRepository.SaveChangesAsync();

        // ---------------- Publicar evento Kafka ----------------
        var evt = new TransferenciaRealizadaEvent
        {
            IdTransferencia = transferencia.IdTransferencia,
            IdContaOrigem = transferencia.IdContaOrigem,
            IdContaDestino = transferencia.IdContaDestino,
            Valor = transferencia.Valor,
            DataMovimento = transferencia.DataMovimento
        };

        await _eventProducer.PublishAsync(evt);

        return Unit.Value;
    }

    Task IRequestHandler<EfetuarTransferenciaCommand>.Handle(EfetuarTransferenciaCommand request, CancellationToken cancellationToken)
    {
        return Handle(request, cancellationToken);
    }
}
