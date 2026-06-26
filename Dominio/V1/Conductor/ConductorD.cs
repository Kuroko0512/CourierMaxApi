namespace Dominio.V1.Conductor
{
    public class ConductorD
    {
        public int Id { get; private set; }

        public string Nombre { get; private set; } = string.Empty;

        public int VehiculoId { get; private set; }

        public bool Activo { get; private set; }

        private ConductorD() { }
    }
}
