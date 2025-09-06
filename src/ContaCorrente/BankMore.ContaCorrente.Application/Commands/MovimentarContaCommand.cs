using MediatR;

namespace BankMore.ContaCorrente.Application.Commands;

public class MovimentarContaCommand : IRequest
{
    public string ChaveIdempotencia { get; set; } = string.Empty;
    public string TipoMovimento { get; set; } = string.Empty; // "C" = Crédito, "D" = Débito
    public decimal Valor { get; set; }
    public string? IdContaDestino { get; set; } // Opcional, usado em transferências
    public string IdContaOrigem { get; set; } = string.Empty; // Obtido do token
}
