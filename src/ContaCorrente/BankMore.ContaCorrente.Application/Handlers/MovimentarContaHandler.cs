using BankMore.ContaCorrente.Application.Commands;
using BankMore.ContaCorrente.Domain.Entities;
using BankMore.ContaCorrente.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BankMore.ContaCorrente.Application.Handlers;

public class MovimentarContaHandler : IRequestHandler<MovimentarContaCommand>
{
    private readonly BancoContext _context;

    public MovimentarContaHandler(BancoContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(MovimentarContaCommand request, CancellationToken cancellationToken)
    {
        // ---------------- Idempotência ----------------
        var existente = await _context.Idempotencias
            .FirstOrDefaultAsync(i => i.ChaveIdempotencia == request.ChaveIdempotencia, cancellationToken);

        if (existente != null)
            return Unit.Value; // Requisição já processada

        // ---------------- Validações ----------------
        var conta = await _context.ContasCorrentes
            .FirstOrDefaultAsync(c => c.IdContaCorrente == request.IdContaOrigem, cancellationToken);

        if (conta == null)
            throw new InvalidOperationException("Conta não cadastrada."); // INVALID_ACCOUNT

        if (!conta.Ativo)
            throw new InvalidOperationException("Conta inativa."); // INACTIVE_ACCOUNT

        if (request.Valor <= 0)
            throw new InvalidOperationException("Valor deve ser positivo."); // INVALID_VALUE

        if (request.TipoMovimento != "C" && request.TipoMovimento != "D")
            throw new InvalidOperationException("Tipo de movimento inválido."); // INVALID_TYPE

        if (request.IdContaDestino != null && request.TipoMovimento != "C")
            throw new InvalidOperationException("Transferência só permite crédito na conta de destino."); // INVALID_TYPE

        // ---------------- Persistir Movimento ----------------
        var movimento = new Movimento
        {
            IdMovimento = Guid.NewGuid().ToString(),
            IdContaCorrente = request.IdContaDestino ?? request.IdContaOrigem,
            TipoMovimento = request.TipoMovimento,
            Valor = request.Valor,
            DataMovimento = DateTime.UtcNow
        };

        _context.Movimentos.Add(movimento);

        // ---------------- Registrar Idempotência ----------------
        var idempotencia = new Idempotencia
        {
            ChaveIdempotencia = request.ChaveIdempotencia,
            Requisicao = $"Origem:{request.IdContaOrigem},Destino:{request.IdContaDestino},Tipo:{request.TipoMovimento},Valor:{request.Valor}",
            Resultado = "Movimento realizado"
        };
        _context.Idempotencias.Add(idempotencia);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    Task IRequestHandler<MovimentarContaCommand>.Handle(MovimentarContaCommand request, CancellationToken cancellationToken)
    {
        return Handle(request, cancellationToken);
    }
}
