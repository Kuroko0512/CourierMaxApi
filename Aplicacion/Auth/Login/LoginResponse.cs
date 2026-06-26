namespace Aplicacion.Auth.Login
{
    public record LoginResponse(int UsuarioId, string Nombre, string Rol, string Token);
}
