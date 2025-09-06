namespace BankMore.Transferencia.Domain.Entities;

public class Transferencia
{
    public string IdTransferencia { get; set; } = Guid.NewGuid().ToString();
    public string IdContaOrigem { get; set; } = string.Empty;
    public string IdContaDestino { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public DateTime DataMovimento { get; set; } = DateTime.UtcNow;
}
