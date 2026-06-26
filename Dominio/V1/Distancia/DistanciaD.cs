namespace Dominio.V1.Distancia
{
    public class DistanciaD
    {
        public int Id { get; private set; }

        public int CiudadOrigenId { get; private set; }

        public int CiudadDestinoId { get; private set; }

        public int DistanciaKm { get; private set; }

        public decimal TarifaDistancia { get; private set; }

        private DistanciaD() { }
    }
}
