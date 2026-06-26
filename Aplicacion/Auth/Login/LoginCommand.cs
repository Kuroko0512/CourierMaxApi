using ErrorOr;
using MediatR;

namespace Aplicacion.Auth.Login
{
    public record LoginCommand(string Usuario, string Password) : IRequest<ErrorOr<LoginResponse>>;
}
