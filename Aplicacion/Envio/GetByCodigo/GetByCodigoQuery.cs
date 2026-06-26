using Aplicacion.Envio.Common;
using ErrorOr;
using MediatR;

namespace Aplicacion.Envio.GetByCodigo
{
    public record GetByCodigoQuery(string CodigoRastreo) : IRequest<ErrorOr<EnvioResponse>>;
}
