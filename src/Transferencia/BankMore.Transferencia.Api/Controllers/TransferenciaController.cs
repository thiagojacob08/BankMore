using BankMore.Transferencia.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BankMore.Transferencia.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Garante que todos endpoints precisem de token JWT
    public class TransferenciaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TransferenciaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> EfetuarTransferencia([FromBody] EfetuarTransferenciaCommand command)
        {
            try
            {
                await _mediator.Send(command);
                return NoContent(); // HTTP 204 em caso de sucesso
            }
            catch (InvalidOperationException ex)
            {
                // Retorna HTTP 400 com a mensagem de falha
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
