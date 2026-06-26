namespace Dominio.V1.TipoPaquete
{
    public class TipoPaqueteD
    {
        public int Id { get; private set; }

        public string Codigo { get; private set; } = string.Empty;

        public string Descripcion { get; private set; } = string.Empty;

        public decimal RecargoPorcentaje { get; private set; }

        public int Orden { get; private set; }

        public bool Activo { get; private set; }

        private TipoPaqueteD() { }
    }
}
