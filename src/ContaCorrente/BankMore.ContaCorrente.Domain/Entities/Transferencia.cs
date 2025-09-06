namespace BankMore.ContaCorrente.Domain.Entities;

public class Transferencia
{
    public string IdTransferencia { get; set; } = Guid.NewGuid().ToString();
    public string IdContaCorrenteOrigem { get; set; }
    public string IdContaCorrenteDestino { get; set; }
    public DateTime DataMovimento { get; set; } = DateTime.UtcNow;
    public decimal Valor { get; set; }

    // Navegação
    public ContaCorrente ContaOrigem { get; set; } = null!;
    public ContaCorrente ContaDestino { get; set; } = null!;

}
