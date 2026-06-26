namespace Dominio.V1.ParametroTarifa
{
    public class ParametroTarifaD
    {
        public int Id { get; private set; }

        public decimal PesoBaseKg { get; private set; }

        public decimal RecargoPorKgAdicional { get; private set; }

        private ParametroTarifaD() { }
    }
}
