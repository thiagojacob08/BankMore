
namespace BankMore.Transferencia.Application.DTOs;

public class TransferenciaDto
{
    public string IdTransferencia { get; set; } = string.Empty;
    public string IdContaOrigem { get; set; } = string.Empty;
    public string IdContaDestino { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public DateTime DataMovimento { get; set; }
}
