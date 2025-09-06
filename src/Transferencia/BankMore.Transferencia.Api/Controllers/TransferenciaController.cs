using BankMore.Transferencia.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankMore.Transferencia.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TransferenciaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TransferenciaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Efetuar([FromBody] EfetuarTransferenciaCommand command)
        {
            try
            {
                // Extrai IdContaOrigem do token
                var idConta = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
                if (string.IsNullOrEmpty(idConta))
                    return Forbid();

                command.IdContaOrigem = idConta;

                await _mediator.Send(command);
                return NoContent(); // 204
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message,
                    type = "INVALID_OPERATION"
                });
            }
        }
    }
}
