namespace BankMore.ContaCorrente.Domain.Entities;

public class Tarifa
{
    public string IdTarifa { get; set; } = Guid.NewGuid().ToString();
    public string IdContaCorrente { get; set; } 
    public DateTime DataMovimento { get; set; } = DateTime.UtcNow;
    public decimal Valor { get; set; }

    // Navegação
    public ContaCorrente ContaCorrente { get; set; } = null!;
}
