using Aplicacion.Metricas.Common;
using Dominio.Servicios;
using Dominio.V1.Conductor;
using Dominio.V1.Envio;
using Dominio.V1.Festivo;
using Dominio.V1.TipoServicio;
using ErrorOr;
using MediatR;

namespace Aplicacion.Metricas.GetMetricasConductores
{
    public class GetMetricasConductoresQueryHandler : IRequestHandler<GetMetricasConductoresQuery, ErrorOr<List<MetricaConductorResponse>>>
    {
        private readonly IEnvioRepository _envioRepository;
        private readonly IConductorRepository _conductorRepository;
        private readonly IFestivoRepository _festivoRepository;
        private readonly ITipoServicioRepository _tipoServicioRepository;

        public GetMetricasConductoresQueryHandler(
            IEnvioRepository envioRepository,
            IConductorRepository conductorRepository,
            IFestivoRepository festivoRepository,
            ITipoServicioRepository tipoServicioRepository)
        {
            _envioRepository = envioRepository ?? throw new ArgumentNullException(nameof(envioRepository));
            _conductorRepository = conductorRepository ?? throw new ArgumentNullException(nameof(conductorRepository));
            _festivoRepository = festivoRepository ?? throw new ArgumentNullException(nameof(festivoRepository));
            _tipoServicioRepository = tipoServicioRepository ?? throw new ArgumentNullException(nameof(tipoServicioRepository));
        }

        public async Task<ErrorOr<List<MetricaConductorResponse>>> Handle(GetMetricasConductoresQuery request, CancellationToken cancellationToken)
        {
            var conductores = await _conductorRepository.GetActivosAsync();
            var envios = await _envioRepository.GetAsignadosAsync(request.Desde, request.Hasta);
            var festivos = (await _festivoRepository.GetAllAsync()).Select(f => f.Fecha).ToHashSet();
            var slaPorTipo = (await _tipoServicioRepository.GetAll()).ToDictionary(t => t.Id, t => t.DiasSla);

            var enviosPorConductor = envios
                .Where(e => e.ConductorId is not null)
                .GroupBy(e => e.ConductorId!.Value)
                .ToDictionary(g => g.Key, g => g.ToList());

            var metricas = new List<MetricaConductorResponse>();

            foreach (var conductor in conductores)
            {
                var misEnvios = enviosPorConductor.TryGetValue(conductor.Id, out var lista)
                    ? lista
                    : new List<EnvioD>();

                var entregados = misEnvios.Where(e => e.Estado == EstadoEnvio.ENTREGADO).ToList();

                var tiempoPromedio = 0m;
                var entregadosConFechas = entregados
                    .Where(e => e.FechaAsignacion is not null && e.FechaEntrega is not null)
                    .ToList();

                if (entregadosConFechas.Count > 0)
                {
                    tiempoPromedio = Math.Round(
                        (decimal)entregadosConFechas.Average(e => (e.FechaEntrega!.Value - e.FechaAsignacion!.Value).TotalDays), 2);
                }

                var dentroSla = 0;
                foreach (var entregado in entregados)
                {
                    if (entregado.FechaEntrega is null)
                    {
                        continue;
                    }

                    if (slaPorTipo.TryGetValue(entregado.TipoServicioId, out var sla) && sla is not null)
                    {
                        var diasHabiles = DiasHabiles.Transcurridos(entregado.FechaCreacion, entregado.FechaEntrega.Value, festivos);
                        if (diasHabiles <= sla.Value)
                        {
                            dentroSla++;
                        }
                    }
                }

                var porcentajeSla = entregados.Count > 0
                    ? Math.Round((decimal)dentroSla / entregados.Count * 100, 2)
                    : 0m;

                var pesoTransportado = misEnvios
                    .Where(e => e.Estado == EstadoEnvio.ENTREGADO || e.Estado == EstadoEnvio.EN_TRANSITO)
                    .Sum(e => e.Paquete.PesoKg);

                metricas.Add(new MetricaConductorResponse(
                    conductor.Id,
                    conductor.Nombre,
                    misEnvios.Count,
                    entregados.Count,
                    misEnvios.Count(e => e.Estado == EstadoEnvio.CANCELADO),
                    misEnvios.Count(e => e.Estado == EstadoEnvio.EN_TRANSITO),
                    tiempoPromedio,
                    porcentajeSla,
                    pesoTransportado));
            }

            return metricas;
        }
    }
}
