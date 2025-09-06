namespace BankMore.ContaCorrente.Application.DTOs;

public class ContaDetalhesDto
{
    public int NumeroConta { get; set; }
    public string Nome { get; set; } = string.Empty;
    public bool Ativo { get; set; }
    public string IdContaCorrente { get; set; }
}
