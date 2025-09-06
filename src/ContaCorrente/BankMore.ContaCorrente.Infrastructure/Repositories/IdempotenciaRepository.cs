using BankMore.ContaCorrente.Domain.Entities;
using BankMore.ContaCorrente.Domain.Interfaces;
using BankMore.ContaCorrente.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BankMore.ContaCorrente.Infrastructure.Repositories;

public class IdempotenciaRepository : IIdempotenciaRepository
{
    private readonly BancoContext _context;
    public IdempotenciaRepository(BancoContext context) => _context = context;

    public async Task<Idempotencia?> GetByChaveAsync(string chave) =>
        await _context.Idempotencias.FirstOrDefaultAsync(i => i.ChaveIdempotencia == chave);

    public async Task AddAsync(Idempotencia idempotencia) =>
        await _context.Idempotencias.AddAsync(idempotencia);
}