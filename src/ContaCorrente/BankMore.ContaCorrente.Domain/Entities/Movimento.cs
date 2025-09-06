namespace BankMore.ContaCorrente.Domain.Entities;

public class Movimento
{
    public string IdMovimento { get; set; } = Guid.NewGuid().ToString();
    public string IdContaCorrente { get; set; } = string.Empty;
    public DateTime DataMovimento { get; set; }
    public string TipoMovimento { get; set; } = "C"; // C = Crédito, D = Débito
    public decimal Valor { get; set; }

    // Navegação
    public ContaCorrente ContaCorrente { get; set; } = null!;
}
