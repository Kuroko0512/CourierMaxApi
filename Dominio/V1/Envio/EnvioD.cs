using Dominio.Primitives;
using Dominio.V1.Envio.ValueObjects;
using ErrorOr;

namespace Dominio.V1.Envio
{
    public class EnvioD : AggregateRoot
    {
        private const string PrefijoCodigo = "CM-";

        public int Id { get; private set; }

        public string CodigoRastreo { get; private set; } = string.Empty;

        public EstadoEnvio Estado { get; private set; }

        public int TipoServicioId { get; private set; }

        public Remitente Remitente { get; private set; } = default!;

        public Destinatario Destinatario { get; private set; } = default!;

        public Paquete Paquete { get; private set; } = default!;

        public int CiudadOrigenId { get; private set; }

        public int CiudadDestinoId { get; private set; }

        public int? ConductorId { get; private set; }

        public decimal? CostoTotal { get; private set; }

        public DateTime FechaCreacion { get; private set; }

        public DateTime? FechaAsignacion { get; private set; }

        public DateTime? FechaEntrega { get; private set; }

        public List<HistorialEstadoD> Historial { get; private set; } = new();

        private EnvioD() { }

        public static EnvioD Crear(
            int tipoServicioId,
            Remitente remitente,
            Destinatario destinatario,
            Paquete paquete,
            int ciudadOrigenId,
            int ciudadDestinoId)
        {
            var envio = new EnvioD
            {
                CodigoRastreo = GenerarCodigo(),
                Estado = EstadoEnvio.CREADO,
                TipoServicioId = tipoServicioId,
                Remitente = remitente,
                Destinatario = destinatario,
                Paquete = paquete,
                CiudadOrigenId = ciudadOrigenId,
                CiudadDestinoId = ciudadDestinoId,
                FechaCreacion = DateTime.Now
            };

            envio.Historial.Add(new HistorialEstadoD(null, EstadoEnvio.CREADO, "Envío creado", null));
            envio.Raise(new EnvioCreadoDomainEvent(Guid.NewGuid(), envio.CodigoRastreo));

            return envio;
        }


        public void CalcularCosto(decimal tarifaBase, decimal tarifaDistancia, decimal recargoPorcentajePaquete, decimal pesoBaseKg, decimal recargoPorKgAdicional)
        {
            var recargoPeso = Math.Max(0m, Paquete.PesoKg - pesoBaseKg) * recargoPorKgAdicional;
            var subtotal = tarifaBase + recargoPeso + tarifaDistancia;
            CostoTotal = Math.Round(subtotal * (1 + recargoPorcentajePaquete / 100m), 2);
        }

        public bool EstaAtrasado(int diasSla, int diasHabilesTranscurridos)
            => Estado != EstadoEnvio.ENTREGADO
               && Estado != EstadoEnvio.CANCELADO
               && diasHabilesTranscurridos > diasSla;

        public ErrorOr<Updated> CambiarEstado(EstadoEnvio nuevoEstado, string? motivo, int? realizadoPorId, int? conductorId = null)
        {
            if (!TransicionPermitida(Estado, nuevoEstado))
            {
                return EnvioErrors.TransicionInvalida(Estado, nuevoEstado);
            }

            if (nuevoEstado == EstadoEnvio.CANCELADO && (string.IsNullOrWhiteSpace(motivo) || motivo.Trim().Length < 5))
            {
                return EnvioErrors.MotivoRequerido;
            }

            if (nuevoEstado == EstadoEnvio.ASIGNADO)
            {
                if (conductorId is null)
                {
                    return EnvioErrors.ConductorRequerido;
                }

                ConductorId = conductorId;
                FechaAsignacion = DateTime.Now;
            }

            if (nuevoEstado == EstadoEnvio.ENTREGADO)
            {
                FechaEntrega = DateTime.Now;
            }

            var anterior = Estado;
            Estado = nuevoEstado;
            Historial.Add(new HistorialEstadoD(anterior, nuevoEstado, motivo, realizadoPorId));

            return Result.Updated;
        }

        private static bool TransicionPermitida(EstadoEnvio actual, EstadoEnvio nuevo) => (actual, nuevo) switch
        {
            (EstadoEnvio.CREADO, EstadoEnvio.ASIGNADO) => true,
            (EstadoEnvio.ASIGNADO, EstadoEnvio.EN_TRANSITO) => true,
            (EstadoEnvio.EN_TRANSITO, EstadoEnvio.ENTREGADO) => true,
            (EstadoEnvio.CREADO, EstadoEnvio.CANCELADO) => true,
            (EstadoEnvio.ASIGNADO, EstadoEnvio.CANCELADO) => true,
            (EstadoEnvio.EN_TRANSITO, EstadoEnvio.CANCELADO) => true,
            _ => false
        };

        private static string GenerarCodigo()
            => $"{PrefijoCodigo}{Random.Shared.Next(0, 100_000_000):D8}";
    }
}
