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
    public ICollection<Movimento> Movimentos { get; private set; } = new List<Movimento>();
    public ICollection<Transferencia> TransferenciasOrigem { get; private set; } = new List<Transferencia>();
    public ICollection<Transferencia> TransferenciasDestino { get; private set; } = new List<Transferencia>();
    public ICollection<Tarifa> Tarifas { get; private set; } = new List<Tarifa>();
    
    public void Ativar() => Ativo = true;
    public void Inativar() => Ativo = false;
}
