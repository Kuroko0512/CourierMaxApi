using Aplicacion.TipoPaquete.GetAll;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CourierMaxApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoPaqueteController : ApiController
    {
        private readonly ISender _mediator;

        public TipoPaqueteController(ISender mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllTipoPaqueteQuery());

            return result.Match(
                tipos => Ok(tipos),
                errors => Problem(errors));
        }
    }
}
