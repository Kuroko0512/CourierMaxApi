using Aplicacion.Envio.Common;
using ErrorOr;
using MediatR;

namespace Aplicacion.Envio.Atrasados
{
    public record GetEnviosAtrasadosQuery(DateTime Desde, DateTime Hasta) : IRequest<ErrorOr<List<EnvioAtrasadoResponse>>>;
}
