using Aplicacion.Metricas.GetMetricasConductores;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CourierMaxApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetricasController : ApiController
    {
        private readonly ISender _mediator;

        public MetricasController(ISender mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet("conductores")]
        public async Task<IActionResult> Conductores([FromQuery] DateTime? desde, [FromQuery] DateTime? hasta)
        {
            var result = await _mediator.Send(new GetMetricasConductoresQuery(desde, hasta));

            return result.Match(
                metricas => Ok(metricas),
                errors => Problem(errors));
        }
    }
}
