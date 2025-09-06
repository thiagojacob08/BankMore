using MediatR;

namespace BankMore.ContaCorrente.Application.Commands;

public class AddTarifaCommand : IRequest
{
    public string IdContaCorrente { get; set; } = string.Empty;
    public decimal Valor { get; set; }
}
