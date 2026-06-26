namespace Dominio.V1.Vehiculo
{
    public class VehiculoD
    {
        public int Id { get; private set; }

        public string Placa { get; private set; } = string.Empty;

        public decimal CapacidadPesoKg { get; private set; }

        public decimal CapacidadVolumenM3 { get; private set; }

        private VehiculoD() { }

        public bool TieneCapacidad(decimal pesoTotal, decimal volumenTotal)
            => pesoTotal <= CapacidadPesoKg && volumenTotal <= CapacidadVolumenM3;
    }
}
