using Dominio.V1.Envio;

namespace Aplicacion.Envio.Common
{
    public record EnvioResponse(
        int Id,
        string CodigoRastreo,
        EstadoEnvio Estado,
        int TipoServicioId,
        RemitenteResponse Remitente,
        DestinatarioResponse Destinatario,
        PaqueteResponse Paquete,
        int CiudadOrigenId,
        int CiudadDestinoId,
        int? ConductorId,
        decimal? CostoTotal,
        DateTime FechaCreacion,
        DateTime? FechaAsignacion,
        DateTime? FechaEntrega,
        List<HistorialEstadoResponse> Historial);

    public record RemitenteResponse(string Nombre, string Telefono, string DireccionRecogida);

    public record DestinatarioResponse(string Nombre, string Telefono, string DireccionEntrega);

    public record PaqueteResponse(decimal PesoKg, decimal LargoCm, decimal AnchoCm, decimal AltoCm, int TipoPaqueteId, decimal VolumenM3);

    public record HistorialEstadoResponse(int Id, EstadoEnvio? EstadoAnterior, EstadoEnvio EstadoNuevo, DateTime FechaCambio, string? Motivo, int? RealizadoPorId);
}
