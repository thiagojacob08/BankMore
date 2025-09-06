using BankMore.ContaCorrente.Application.DTOs;
using MediatR;

namespace BankMore.ContaCorrente.Application.Queries;

public class LoginContaCorrenteQuery : IRequest<LoginContaCorrenteDto>
{
    public string? CPF { get; set; }
    public int? NumeroConta { get; set; }
    public string Senha { get; set; } = string.Empty;
}


