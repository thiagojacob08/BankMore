using BankMore.ContaCorrente.Domain.Enums;
using MediatR;

namespace BankMore.ContaCorrente.Application.Commands;

public class MovimentarContaCommand : IRequest
{
    public string? ChaveIdempotencia { get; set; }
    public TipoMovimentacao TipoMovimento { get; set; }// "C" = Crédito, "D" = Débito
    public decimal Valor { get; set; }
    public string? IdContaDestino { get; set; } // Opcional, usado em transferências
    public string? IdContaOrigem { get; set; } // Obtido do token
}
