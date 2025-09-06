using MediatR;

namespace BankMore.ContaCorrente.Application.Queries;

public class LoginContaCorrenteQuery : IRequest<LoginContaCorrenteResult>
{
    public string? CPF { get; set; }
    public int? NumeroConta { get; set; }
    public string Senha { get; set; } = string.Empty;
}

public class LoginContaCorrenteResult
{
    public string Token { get; set; } = string.Empty;
    public string IdContaCorrente { get; set; } = string.Empty;
}
