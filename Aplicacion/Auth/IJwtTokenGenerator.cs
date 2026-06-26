namespace Aplicacion.Auth
{
    public interface IJwtTokenGenerator
    {
        string Generate(int usuarioId, string nombre, string rol);
    }
}
