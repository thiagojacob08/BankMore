using BankMore.ContaCorrente.Domain.Entities;
using BankMore.ContaCorrente.Domain.Interfaces;
using BankMore.ContaCorrente.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BankMore.ContaCorrente.Infrastructure.Repositories;

public class TransferenciaRepository : ITransferenciaRepository
{
    private readonly BancoContext _context;
    public TransferenciaRepository(BancoContext context) => _context = context;

    public async Task AddAsync(Transferencia transferencia) =>
        await _context.Transferencias.AddAsync(transferencia);

    public async Task<IEnumerable<Transferencia>> GetByContaIdAsync(string idContaCorrente) =>
        await _context.Transferencias
            .Where(t => t.IdContaCorrenteOrigem == idContaCorrente || t.IdContaCorrenteDestino == idContaCorrente)
            .ToListAsync();
}
