using Aplicacion.Rol.GetAll;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CourierMaxApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolController : ApiController
    {
        private readonly ISender _mediator;

        public RolController(ISender mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllRolQuery());

            return result.Match(
                roles => Ok(roles),
                errors => Problem(errors));
        }
    }
}
