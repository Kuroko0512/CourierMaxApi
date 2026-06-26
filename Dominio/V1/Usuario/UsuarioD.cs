using Dominio.V1.Rol;

namespace Dominio.V1.Usuario
{
    public class UsuarioD
    {
        public int Id { get; private set; }

        public string Nombre { get; private set; } = string.Empty;

        public int RolId { get; private set; }

        public RolD? Rol { get; private set; }

        private UsuarioD() { }
    }
}
