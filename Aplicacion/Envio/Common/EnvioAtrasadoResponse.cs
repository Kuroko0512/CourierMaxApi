using Dominio.V1.Envio;

namespace Aplicacion.Envio.Common
{
    public record EnvioAtrasadoResponse(
        int Id,
        string CodigoRastreo,
        EstadoEnvio Estado,
        int TipoServicioId,
        DateTime FechaCreacion,
        int DiasSla,
        int DiasHabilesTranscurridos,
        int DiasDeAtraso);
}
