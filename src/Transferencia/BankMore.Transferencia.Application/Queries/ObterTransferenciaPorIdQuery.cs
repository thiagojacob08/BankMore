using BankMore.Transferencia.Application.DTOs;
using MediatR;

namespace BankMore.Transferencia.Application.Queries;

public class ObterTransferenciaPorIdQuery : IRequest<TransferenciaDto>
{
    public string IdTransferencia { get; set; } = string.Empty;
}
