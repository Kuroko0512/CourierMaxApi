using Aplicacion.TipoServicio.GetAll;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CourierMaxApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoServicioController : ApiController
    {
        private readonly ISender _mediator;

        public TipoServicioController(ISender mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllTipoServicioQuery());

            return result.Match(
                tipos => Ok(tipos),
                errors => Problem(errors));
        }
    }
}
