using Aplicacion.Envio.Common;
using ErrorOr;
using MediatR;

namespace Aplicacion.Envio.GetAll
{
    public record GetAllEnviosQuery() : IRequest<ErrorOr<List<EnvioResponse>>>;
}
