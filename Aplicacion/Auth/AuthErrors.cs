using ErrorOr;

namespace Aplicacion.Auth
{
    public static class AuthErrors
    {
        public static Error CredencialesInvalidas =>
            Error.Unauthorized("Auth.CredencialesInvalidas", "Usuario o contraseña inválidos.");
    }
}
