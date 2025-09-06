using BankMore.ContaCorrente.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankMore.ContaCorrente.Application.Queries;

public class ObterContaPorNumeroQuery : IRequest<ContaDetalhesDto>
{
    public int numeroContaCorrente { get; set; }
}


