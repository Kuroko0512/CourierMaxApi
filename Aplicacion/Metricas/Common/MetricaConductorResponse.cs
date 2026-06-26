namespace Aplicacion.Metricas.Common
{
    public record MetricaConductorResponse(
        int ConductorId,
        string Nombre,
        int TotalAsignados,
        int Entregados,
        int Cancelados,
        int EnTransito,
        decimal TiempoPromedioEntregaDias,
        decimal PorcentajeDentroSla,
        decimal PesoTotalTransportado);
}
