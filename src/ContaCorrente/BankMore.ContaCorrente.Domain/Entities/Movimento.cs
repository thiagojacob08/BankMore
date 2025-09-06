namespace BankMore.ContaCorrente.Domain.Entities;

public class Movimento
{
    public string IdMovimento { get; set; } = Guid.NewGuid().ToString();
    public string? IdContaCorrente { get; set; }
    public DateTime DataMovimento { get; set; } = DateTime.UtcNow;
    public Enums.TipoMovimentacao TipoMovimento { get; set; } // C = Crédito, D = Débito
    public decimal Valor { get; set; }

    // Navegação
    public ContaCorrente ContaCorrente { get; private set; } = null!;
        
}
