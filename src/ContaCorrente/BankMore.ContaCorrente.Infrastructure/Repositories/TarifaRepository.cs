using BankMore.ContaCorrente.Domain.Entities;
using BankMore.ContaCorrente.Domain.Interfaces;
using BankMore.ContaCorrente.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BankMore.ContaCorrente.Infrastructure.Repositories;

public class TarifaRepository : ITarifaRepository
{
    private readonly BancoContext _context;
    public TarifaRepository(BancoContext context) => _context = context;

    public async Task AddAsync(Tarifa tarifa) =>
        await _context.Tarifas.AddAsync(tarifa);

    public async Task<IEnumerable<Tarifa>> GetByContaIdAsync(string idContaCorrente) =>
        await _context.Tarifas
            .Where(t => t.IdContaCorrente == idContaCorrente)
            .ToListAsync();
}
