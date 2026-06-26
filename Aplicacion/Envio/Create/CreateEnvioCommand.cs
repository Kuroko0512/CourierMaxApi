using Aplicacion.Envio.Common;
using ErrorOr;
using MediatR;

namespace Aplicacion.Envio.Create
{
    public record CreateEnvioCommand(
        int TipoServicioId,
        string RemitenteNombre,
        string RemitenteTelefono,
        string RemitenteDireccionRecogida,
        string DestinatarioNombre,
        string DestinatarioTelefono,
        string DestinatarioDireccionEntrega,
        decimal PesoKg,
        decimal LargoCm,
        decimal AnchoCm,
        decimal AltoCm,
        int TipoPaqueteId,
        int CiudadOrigenId,
        int CiudadDestinoId) : IRequest<ErrorOr<EnvioResponse>>;
}
