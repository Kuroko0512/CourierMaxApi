namespace Aplicacion.Rol.Common
{
    public record RolResponse(
        int Id,
        string Codigo,
        string Descripcion,
        int Orden,
        bool Activo);
}
