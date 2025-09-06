using BankMore.ContaCorrente.Domain.Entities;

namespace BankMore.ContaCorrente.Domain.Interfaces;

public interface IMovimentoRepository
{
    Task AddAsync(Movimento movimento);
}
