using MediatR;

namespace BankMore.ContaCorrente.Application.Queries;

public class ConsultarSaldoQuery : IRequest<ConsultarSaldoResult>
{
    public string IdContaCorrente { get; set; } = string.Empty; // Obtido do token
}

public class ConsultarSaldoResult
{
    public int NumeroConta { get; set; }
    public string Nome { get; set; } = string.Empty;
    public DateTime DataHoraConsulta { get; set; }
    public decimal Saldo { get; set; }
}
