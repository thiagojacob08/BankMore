using BankMore.ContaCorrente.Domain.Entities;

namespace BankMore.ContaCorrente.Domain.Interfaces;

public interface ITransferenciaRepository
{
    Task AddAsync(Transferencia transferencia);
    Task<IEnumerable<Transferencia>> GetByContaIdAsync(string idContaCorrente);
}
