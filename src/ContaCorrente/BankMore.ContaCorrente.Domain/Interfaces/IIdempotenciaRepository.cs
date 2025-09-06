using BankMore.ContaCorrente.Domain.Entities;

namespace BankMore.ContaCorrente.Domain.Interfaces;

public interface IIdempotenciaRepository
{
    Task<Idempotencia?> GetByChaveAsync(string chave);
    Task AddAsync(Idempotencia idempotencia);
}
