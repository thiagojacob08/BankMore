using BankMore.Transferencia.Domain.Entities;

namespace BankMore.Transferencia.Domain.Interfaces;

public interface IIdempotenciaRepository
{
    Task<Idempotencia?> GetByChaveAsync(string chaveIdempotencia);
    Task AddAsync(Idempotencia idempotencia);
    Task UpdateResultadoAsync(string chaveIdempotencia, string resultado);
    Task SaveChangesAsync();
}
