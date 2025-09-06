using BankMore.ContaCorrente.Application.Commands;
using BankMore.ContaCorrente.Application.DTOs;
using BankMore.ContaCorrente.Application.Queries;

namespace BankMore.ContaCorrente.Application.Services;

public interface IContaCorrenteService
{
    Task<CreateContaCorrenteResult> CadastrarContaAsync(CreateContaCorrenteCommand command);
    Task<LoginContaCorrenteDto> LoginAsync(LoginContaCorrenteQuery query);
    Task MovimentarContaAsync(MovimentarContaCommand command, string idContaOrigem);
    Task<ConsultarSaldoDto> ConsultarSaldoAsync(string idConta);
    Task<ContaDetalhesDto> ObterContaPorNumeroAsync(int numeroConta);
}
