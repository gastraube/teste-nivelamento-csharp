using Castle.Core.Resource;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests;

namespace Questao5.Infrastructure.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaldoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SaldoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> GetSaldoByConta(GetSaldoRequest command)
        {
            try
            {
                var saldo = await _mediator.Send(command);

                return Ok(saldo);
            }
            catch (Exception ex) //tratar excessao
            {
                return NotFound(ex.Message);
            } 
        }
    }
}
