using BankMore.ContaCorrente.Application.Queries;
using BankMore.ContaCorrente.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BankMore.ContaCorrente.Application.Handlers;

public class ConsultarSaldoHandler : IRequestHandler<ConsultarSaldoQuery, ConsultarSaldoResult>
{
    private readonly BancoContext _context;

    public ConsultarSaldoHandler(BancoContext context)
    {
        _context = context;
    }

    public async Task<ConsultarSaldoResult> Handle(ConsultarSaldoQuery request, CancellationToken cancellationToken)
    {
        var conta = await _context.ContasCorrentes
            .Include(c => c.Movimentos)
            .FirstOrDefaultAsync(c => c.IdContaCorrente == request.IdContaCorrente, cancellationToken);

        if (conta == null)
            throw new InvalidOperationException("Conta não cadastrada."); // INVALID_ACCOUNT

        if (!conta.Ativo)
            throw new InvalidOperationException("Conta inativa."); // INACTIVE_ACCOUNT

        // Calcular saldo = soma créditos - soma débitos
        var saldo = conta.Movimentos
            .Sum(m => m.TipoMovimento == "C" ? m.Valor : -m.Valor);

        return new ConsultarSaldoResult
        {
            NumeroConta = conta.Numero,
            Nome = conta.Nome,
            DataHoraConsulta = DateTime.UtcNow,
            Saldo = saldo
        };
    }
}