using MediatR;

namespace BankMore.Transferencia.Application.Commands;

public class EfetuarTransferenciaCommand : IRequest
{
    public string ChaveIdempotencia { get; set; } = string.Empty;
    public string IdContaOrigem { get; set; } = string.Empty;
    public string IdContaDestino { get; set; } = string.Empty;
    public decimal Valor { get; set; }
}