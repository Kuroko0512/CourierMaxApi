namespace Aplicacion.TipoPaquete.Common
{
    public record TipoPaqueteResponse(
        int Id,
        string Codigo,
        string Descripcion,
        decimal RecargoPorcentaje,
        int Orden,
        bool Activo);
}
