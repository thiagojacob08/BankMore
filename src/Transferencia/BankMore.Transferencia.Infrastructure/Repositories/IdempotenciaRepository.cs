using BankMore.Transferencia.Domain.Entities;
using BankMore.Transferencia.Domain.Interfaces;
using BankMore.Transferencia.Infrastructure.Data;

namespace BankMore.Transferencia.Infrastructure.Repositories;

public class IdempotenciaRepository : IIdempotenciaRepository
{
    private readonly TransferenciaContext _context;

    public IdempotenciaRepository(TransferenciaContext context)
    {
        _context = context;
    }

    public async Task<Idempotencia?> GetByChaveAsync(string chaveIdempotencia) =>
        await _context.Idempotencias.FindAsync(chaveIdempotencia);

    public async Task AddAsync(Idempotencia idempotencia) =>
        await _context.Idempotencias.AddAsync(idempotencia);

    public async Task UpdateResultadoAsync(string chaveIdempotencia, string resultado)
    {
        var entity = await _context.Idempotencias.FindAsync(chaveIdempotencia);
        if (entity != null)
        {
            entity.Resultado = resultado;
            _context.Idempotencias.Update(entity);
        }
    }

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}
