namespace Dominio.V1.Envio
{

    public class HistorialEstadoD
    {
        public int Id { get; private set; }

        public int EnvioId { get; private set; }

        public EstadoEnvio? EstadoAnterior { get; private set; }

        public EstadoEnvio EstadoNuevo { get; private set; }

        public DateTime FechaCambio { get; private set; }

        public string? Motivo { get; private set; }

        public int? RealizadoPorId { get; private set; }

        private HistorialEstadoD() { }

        public HistorialEstadoD(EstadoEnvio? estadoAnterior, EstadoEnvio estadoNuevo, string? motivo, int? realizadoPorId)
        {
            EstadoAnterior = estadoAnterior;
            EstadoNuevo = estadoNuevo;
            Motivo = motivo;
            RealizadoPorId = realizadoPorId;
            FechaCambio = DateTime.Now;
        }
    }
}
