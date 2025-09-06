using BankMore.ContaCorrente.Application.Commands;
using BankMore.ContaCorrente.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankMore.ContaCorrente.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContaCorrenteController : ControllerBase
{
    private readonly IMediator _mediator;

    public ContaCorrenteController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // POST: api/ContaCorrente
    [HttpPost("cadastrar")]
    public async Task<IActionResult> Cadastrar([FromBody] CreateContaCorrenteCommand command)
    {
        // Validação básica do CPF (opcional)
        if (!IsCpfValid(command.CPF))
        {
            return BadRequest(new
            {
                message = "CPF inválido.",
                type = "INVALID_DOCUMENT"
            });
        }

        try
        {
            var result = await _mediator.Send(command);

            return CreatedAtAction(nameof(Obter), new { numeroConta = result.NumeroConta }, new
            {
                numeroConta = result.NumeroConta,
                idContaCorrente = result.IdContaCorrente
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                message = ex.Message,
                type = "ERROR"
            });
        }
    }

    // GET: api/ContaCorrente/{numeroConta}
    [HttpGet("{numeroConta}")]
    public async Task<IActionResult> Obter(int numeroConta)
    {
        // Aqui você pode implementar a lógica para retornar a conta pelo número
        return Ok(new { message = "Endpoint de exemplo" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginContaCorrenteQuery query)
    {
        try
        {
            var result = await _mediator.Send(query);
            return Ok(new
            {
                token = result.Token,
                idContaCorrente = result.IdContaCorrente
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new
            {
                message = ex.Message,
                type = "USER_UNAUTHORIZED"
            });
        }
    }

    [HttpPost("movimentar")]
    [Authorize]
    public async Task<IActionResult> Movimentar([FromBody] MovimentarContaCommand command)
    {
        // Pega IdContaOrigem do token
        var idConta = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
        if (string.IsNullOrEmpty(idConta))
            return Forbid();

        command.IdContaOrigem = idConta;

        try
        {
            await _mediator.Send(command);
            return NoContent(); // 204
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                message = ex.Message,
                type = ex.Message switch
                {
                    "Conta não cadastrada." => "INVALID_ACCOUNT",
                    "Conta inativa." => "INACTIVE_ACCOUNT",
                    "Valor deve ser positivo." => "INVALID_VALUE",
                    "Tipo de movimento inválido." => "INVALID_TYPE",
                    "Transferência só permite crédito na conta de destino." => "INVALID_TYPE",
                    _ => "ERROR"
                }
            });
        }
    }

    [HttpGet("saldo")]
    [Authorize]
    public async Task<IActionResult> ConsultarSaldo()
    {
        // Pega IdContaCorrente do token
        var idConta = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
        if (string.IsNullOrEmpty(idConta))
            return Forbid();

        var query = new ConsultarSaldoQuery { IdContaCorrente = idConta };

        try
        {
            var result = await _mediator.Send(query);

            return Ok(new
            {
                numeroConta = result.NumeroConta,
                nome = result.Nome,
                dataHoraConsulta = result.DataHoraConsulta,
                saldo = result.Saldo
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                message = ex.Message,
                type = ex.Message switch
                {
                    "Conta não cadastrada." => "INVALID_ACCOUNT",
                    "Conta inativa." => "INACTIVE_ACCOUNT",
                    _ => "ERROR"
                }
            });
        }
    }


    // ------------------ Helpers ------------------
    private bool IsCpfValid(string cpf)
    {
        // Implementação simples de validação de CPF (apenas tamanho e dígitos)
        if (string.IsNullOrWhiteSpace(cpf)) return false;
        var digits = cpf.Where(char.IsDigit).ToArray();
        return digits.Length == 11;
    }
}
