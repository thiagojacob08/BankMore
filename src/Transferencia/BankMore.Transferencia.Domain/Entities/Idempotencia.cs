
namespace BankMore.Transferencia.Domain.Entities;

public class Idempotencia
{
    public string ChaveIdempotencia { get; set; } = Guid.NewGuid().ToString();
    public string Requisicao { get; set; } = string.Empty; // JSON ou string resumida da requisição
    public string Resultado { get; set; } = string.Empty; // Sucesso ou falha
}
