
namespace BankMore.ContaCorrente.Domain.Interfaces;
public interface IContaCorrenteRepository
{
    Task<Entities.ContaCorrente?> GetByIdAsync(Guid id);
    Task<Entities.ContaCorrente?> GetByNumeroAsync(int numero);
    Task AddAsync(Entities.ContaCorrente conta);
    Task SaveChangesAsync();
}
