using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CourierMaxApi.Controllers
{
    [AllowAnonymous]
    public class ErrorsController : ControllerBase
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("/error")]

        public IActionResult Error()
        {
            Exception? exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

            return Problem();
        }
    }
}
