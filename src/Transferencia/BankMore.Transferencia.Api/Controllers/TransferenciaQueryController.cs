using BankMore.Transferencia.Application.DTOs;
using BankMore.Transferencia.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankMore.Transferencia.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Protege todos endpoints com JWT
public class TransferenciaQueryController : ControllerBase
{
    private readonly IMediator _mediator;

    public TransferenciaQueryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET: api/TransferenciaQuery/conta/{idConta}
    [HttpGet("conta/{idConta}")]
    public async Task<ActionResult<IEnumerable<TransferenciaDto>>> ObterPorConta(string idConta)
    {
        var query = new ObterTransferenciasPorContaQuery { IdConta = idConta };
        var result = await _mediator.Send(query);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    // GET: api/TransferenciaQuery/{idTransferencia}
    [HttpGet("{idTransferencia}")]
    public async Task<ActionResult<TransferenciaDto>> ObterPorId(string idTransferencia)
    {
        var query = new ObterTransferenciaPorIdQuery { IdTransferencia = idTransferencia };
        var result = await _mediator.Send(query);

        if (result == null)
            return NotFound();

        return Ok(result);
    }
}
