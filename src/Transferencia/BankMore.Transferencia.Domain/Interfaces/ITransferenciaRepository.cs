

namespace BankMore.Transferencia.Domain.Interfaces;

public interface ITransferenciaRepository
{
    Task<Domain.Entities.Transferencia?> GetByIdAsync(string idTransferencia);
    Task<IEnumerable<Domain.Entities.Transferencia>> GetAllAsync();
    Task AddAsync(Domain.Entities.Transferencia transferencia);
    Task SaveChangesAsync();
    Task<IEnumerable<Domain.Entities.Transferencia>> GetByContaAsync(string idConta);
}
