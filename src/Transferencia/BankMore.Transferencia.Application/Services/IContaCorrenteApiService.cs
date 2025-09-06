
namespace BankMore.Transferencia.Application.Services;

public interface IContaCorrenteApiService
{
    Task MovimentarDebitoAsync(string idConta, decimal valor, string chaveIdempotencia, CancellationToken cancellationToken = default);
    Task MovimentarCreditoAsync(string idContaDestino, decimal valor, string chaveIdempotencia, CancellationToken cancellationToken = default);
}
