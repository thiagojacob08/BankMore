using BankMore.ContaCorrente.Domain.Entities;

namespace BankMore.ContaCorrente.Domain.Interfaces;

public interface ITarifaRepository
{
    Task AddAsync(Tarifa tarifa);
    Task<IEnumerable<Tarifa>> GetByContaIdAsync(string idContaCorrente);
}