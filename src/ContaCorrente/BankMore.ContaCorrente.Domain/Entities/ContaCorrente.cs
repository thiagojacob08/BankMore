namespace BankMore.ContaCorrente.Domain.Entities;

public class ContaCorrente
{
    public string IdContaCorrente { get; set; } = Guid.NewGuid().ToString();
    public int Numero { get; set; }
    public string Nome { get; set; } = string.Empty;
    public bool Ativo { get; set; } = true;
    public string Senha { get; set; } = string.Empty;
    public string Salt { get; set; } = string.Empty;

    // Navegações
    public ICollection<Movimento> Movimentos { get; set; } = new List<Movimento>();
    public ICollection<Transferencia> TransferenciasOrigem { get; set; } = new List<Transferencia>();
    public ICollection<Transferencia> TransferenciasDestino { get; set; } = new List<Transferencia>();
    public ICollection<Tarifa> Tarifas { get; set; } = new List<Tarifa>();
}
