using Aplicacion.Envio.Common;
using Dominio.V1.Envio;
using ErrorOr;
using MediatR;

namespace Aplicacion.Envio.CambiarEstado
{
    public record CambiarEstadoCommand(
        int Id,
        EstadoEnvio NuevoEstado,
        string? Motivo) : IRequest<ErrorOr<EnvioResponse>>;
}
