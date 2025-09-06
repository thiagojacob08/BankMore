using BankMore.ContaCorrente.Application.Commands;
using BankMore.ContaCorrente.Domain.Entities;
using BankMore.ContaCorrente.Domain.Interfaces;
using MediatR;

namespace BankMore.ContaCorrente.Application.Handlers;

public class AddTransferenciaHandler : IRequestHandler<AddTransferenciaCommand>
{
    private readonly ITransferenciaRepository _transferenciaRepository;

    public AddTransferenciaHandler(ITransferenciaRepository transferenciaRepository)
    {
        _transferenciaRepository = transferenciaRepository;
    }

    public async Task<Unit> Handle(AddTransferenciaCommand request, CancellationToken cancellationToken)
    {
        var transferencia = new Transferencia
        {
            IdTransferencia = Guid.NewGuid().ToString(),
            IdContaCorrenteOrigem = request.IdContaOrigem,
            IdContaCorrenteDestino = request.IdContaDestino,
            Valor = request.Valor,
            DataMovimento = DateTime.UtcNow
        };

        await _transferenciaRepository.AddAsync(transferencia);
        return Unit.Value;
    }

    Task IRequestHandler<AddTransferenciaCommand>.Handle(AddTransferenciaCommand request, CancellationToken cancellationToken)
    {
        return Handle(request, cancellationToken);
    }
}
