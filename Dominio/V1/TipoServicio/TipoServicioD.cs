namespace Dominio.V1.TipoServicio
{
    public class TipoServicioD
    {
        public int Id { get; private set; }

        public string Codigo { get; private set; } = string.Empty;

        public string Descripcion { get; private set; } = string.Empty;

        public decimal TarifaBase { get; private set; }

        public int? DiasSla { get; private set; }

        public int Orden { get; private set; }

        public bool Activo { get; private set; }

        private TipoServicioD() { }
    }
}
