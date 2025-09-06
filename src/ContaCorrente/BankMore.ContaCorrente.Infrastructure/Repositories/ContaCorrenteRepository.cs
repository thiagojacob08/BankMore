using BankMore.ContaCorrente.Domain.Interfaces;
using BankMore.ContaCorrente.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BankMore.ContaCorrente.Infrastructure.Repositories;

public class ContaCorrenteRepository : IContaCorrenteRepository
{
    private readonly BancoContext _context;
    public ContaCorrenteRepository(BancoContext context) => _context = context;

    public async Task<Domain.Entities.ContaCorrente?> GetByIdAsync(Guid id) =>
        await _context.ContasCorrentes.FindAsync(id);

    public async Task<Domain.Entities.ContaCorrente?> GetByNumeroAsync(int numero) =>
        await _context.ContasCorrentes.FirstOrDefaultAsync(c => c.Numero == numero);

    public async Task AddAsync(Domain.Entities.ContaCorrente conta) =>
        await _context.ContasCorrentes.AddAsync(conta);

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}
