using MediatR;

namespace BankMore.ContaCorrente.Application.Commands;

public class CreateContaCorrenteCommand : IRequest<CreateContaCorrenteResult>
{
    public string Nome { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty; // Opcional para validação extra
    public string Senha { get; set; } = string.Empty;
}

public class CreateContaCorrenteResult
{
    public int NumeroConta { get; set; }
    public string IdContaCorrente { get; set; } = string.Empty;
}
