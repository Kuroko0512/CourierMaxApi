namespace Aplicacion.TipoServicio.Common
{
    public record TipoServicioResponse(
        int Id,
        string Codigo,
        string Descripcion,
        decimal TarifaBase,
        int? DiasSla,
        int Orden,
        bool Activo);
}
