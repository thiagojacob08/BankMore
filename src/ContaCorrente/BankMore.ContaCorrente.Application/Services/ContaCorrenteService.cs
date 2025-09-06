using BankMore.ContaCorrente.Application.Commands;
using BankMore.ContaCorrente.Application.DTOs;
using BankMore.ContaCorrente.Application.Queries;
using MediatR;

namespace BankMore.ContaCorrente.Application.Services;

public class ContaCorrenteService : IContaCorrenteService
{
    private readonly IMediator _mediator;

    public ContaCorrenteService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<CreateContaCorrenteResult> CadastrarContaAsync(CreateContaCorrenteCommand command)
    {
        if (!IsCpfValid(command.CPF))
            throw new InvalidOperationException("CPF inválido.");

        return await _mediator.Send(command);
    }

    public async Task<LoginContaCorrenteDto> LoginAsync(LoginContaCorrenteQuery query)
    {
        return await _mediator.Send(query);
    }

    public async Task MovimentarContaAsync(MovimentarContaCommand command, string idContaOrigem)
    {
        command.IdContaOrigem = idContaOrigem;
        await _mediator.Send(command);
    }

    public async Task<ConsultarSaldoDto> ConsultarSaldoAsync(string idConta)
    {
        var query = new ConsultarSaldoQuery { IdContaCorrente = idConta };
        return await _mediator.Send(query);
    }

    public async Task<ContaDetalhesDto> ObterContaPorNumeroAsync(int numeroConta)
    {
        var query = new ObterContaPorNumeroQuery { numeroContaCorrente = numeroConta };
        return await _mediator.Send(query);
    }

    private bool IsCpfValid(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf)) return false;
        var digits = cpf.Where(char.IsDigit).ToArray();
        return digits.Length == 11;
    }
}
