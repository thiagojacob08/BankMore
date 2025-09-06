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

    // ------------------ Conta ------------------
    [HttpPost("cadastrar")]
    public async Task<IActionResult> Cadastrar([FromBody] CreateContaCorrenteCommand command)
    {
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
            return BadRequest(new { message = ex.Message, type = "ERROR" });
        }
    }

    [HttpGet("{numeroConta}")]
    public async Task<IActionResult> Obter(int numeroConta)
    {
        var query = new ObterContaPorNumeroQuery { numeroContaCorrente = numeroConta };
        var conta = await _mediator.Send(query);
        return Ok(conta);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginContaCorrenteQuery query)
    {
        try
        {
            var result = await _mediator.Send(query);
            return Ok(new { token = result.Token, idContaCorrente = result.IdContaCorrente });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message, type = "USER_UNAUTHORIZED" });
        }
    }

    // ------------------ Movimentação ------------------
    [HttpPost("movimentar")]
    [Authorize]
    public async Task<IActionResult> Movimentar([FromBody] MovimentarContaCommand command)
    {
        var idConta = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
        if (string.IsNullOrEmpty(idConta))
            return Forbid();

        command.IdContaOrigem = idConta;

        try
        {
            await _mediator.Send(command);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message, type = "INVALID_OPERATION" });
        }
    }

    [HttpGet("saldo")]
    [Authorize]
    public async Task<IActionResult> ConsultarSaldo()
    {
        var idConta = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
        if (string.IsNullOrEmpty(idConta))
            return Forbid();

        var query = new ConsultarSaldoQuery { IdContaCorrente = idConta };
        var result = await _mediator.Send(query);
        return Ok(new
        {
            numeroConta = result.NumeroConta,
            nome = result.Nome,
            dataHoraConsulta = result.DataHoraConsulta,
            saldo = result.Saldo
        });
    }

    // ------------------ Tarifa ------------------
    [HttpPost("tarifa")]
    [Authorize]
    public async Task<IActionResult> AdicionarTarifa([FromBody] AddTarifaCommand command)
    {
        try
        {
            await _mediator.Send(command);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message, type = "ERROR" });
        }
    }

    // ------------------ Transferência ------------------
    [HttpPost("transferencia")]
    [Authorize]
    public async Task<IActionResult> AdicionarTransferencia([FromBody] AddTransferenciaCommand command)
    {
        try
        {
            await _mediator.Send(command);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message, type = "INVALID_OPERATION" });
        }
    }
}
