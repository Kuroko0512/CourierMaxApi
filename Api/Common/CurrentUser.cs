using Aplicacion.Auth;
using System.Security.Claims;

namespace CourierMaxApi.Common
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int? UsuarioId
        {
            get
            {
                var user = _httpContextAccessor.HttpContext?.User;

                var sub = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                          ?? user?.FindFirst("sub")?.Value;

                return int.TryParse(sub, out var id) ? id : null;
            }
        }
    }
}
