using BankMore.Transferencia.Application.DTOs;
using MediatR;

namespace BankMore.Transferencia.Application.Queries;

public class ObterTransferenciasPorContaQuery : IRequest<IEnumerable<TransferenciaDto>>
{
    public string IdConta { get; set; } = string.Empty;
}
