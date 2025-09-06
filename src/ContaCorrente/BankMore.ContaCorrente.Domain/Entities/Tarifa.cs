namespace BankMore.ContaCorrente.Domain.Entities;

public class Tarifa
{
    public string IdTarifa { get; set; } = Guid.NewGuid().ToString();
    public string IdContaCorrente { get; set; } = string.Empty;
    public DateTime DataMovimento { get; set; }
    public decimal Valor { get; set; }

    // Navegação
    public ContaCorrente ContaCorrente { get; set; } = null!;
}
