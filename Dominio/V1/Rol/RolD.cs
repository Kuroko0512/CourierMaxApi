namespace Dominio.V1.Rol
{
    public class RolD
    {
        public int Id { get; private set; }

        public string Codigo { get; private set; } = string.Empty;

        public string Descripcion { get; private set; } = string.Empty;

        public int Orden { get; private set; }

        public bool Activo { get; private set; }

        private RolD() { }
    }
}
