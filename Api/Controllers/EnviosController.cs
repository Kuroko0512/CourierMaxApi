using Aplicacion.Envio.Atrasados;
using Aplicacion.Envio.CambiarEstado;
using Aplicacion.Envio.Create;
using Aplicacion.Envio.GetAll;
using Aplicacion.Envio.GetByCodigo;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourierMaxApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnviosController : ApiController
    {
        private readonly ISender _mediator;

        public EnviosController(ISender mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CreateEnvioCommand command)
        {
            var result = await _mediator.Send(command);

            return result.Match(
                envio => Ok(envio),
                errors => Problem(errors));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllEnviosQuery());

            return result.Match(
                envios => Ok(envios),
                errors => Problem(errors));
        }

        [HttpGet("{codigo}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByCodigo(string codigo)
        {
            var result = await _mediator.Send(new GetByCodigoQuery(codigo));

            return result.Match(
                envio => Ok(envio),
                errors => Problem(errors));
        }

        [HttpPatch("{id:int}/estado")]
        public async Task<IActionResult> CambiarEstado(int id, [FromBody] CambiarEstadoCommand command)
        {
            var result = await _mediator.Send(command with { Id = id });

            return result.Match(
                envio => Ok(envio),
                errors => Problem(errors));
        }

        [HttpGet("atrasados")]
        public async Task<IActionResult> GetAtrasados([FromQuery] DateTime desde, [FromQuery] DateTime hasta)
        {
            var result = await _mediator.Send(new GetEnviosAtrasadosQuery(desde, hasta));

            return result.Match(
                atrasados => Ok(atrasados),
                errors => Problem(errors));
        }
    }
}
