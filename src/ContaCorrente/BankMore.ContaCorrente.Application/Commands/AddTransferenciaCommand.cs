using MediatR;

namespace BankMore.ContaCorrente.Application.Commands;

public class AddTransferenciaCommand : IRequest
{
    public string IdContaOrigem { get; set; } = string.Empty;
    public string IdContaDestino { get; set; } = string.Empty;
    public decimal Valor { get; set; }
}
