using BankMore.ContaCorrente.Application.DTOs;
using MediatR;

namespace BankMore.ContaCorrente.Application.Queries;

public class ConsultarSaldoQuery : IRequest<ConsultarSaldoDto>
{
    public string IdContaCorrente { get; set; }
}


