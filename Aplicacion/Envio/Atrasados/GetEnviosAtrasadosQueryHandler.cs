using Aplicacion.Envio.Common;
using Dominio.Servicios;
using Dominio.V1.Envio;
using Dominio.V1.Festivo;
using Dominio.V1.TipoServicio;
using ErrorOr;
using MediatR;

namespace Aplicacion.Envio.Atrasados
{
    public class GetEnviosAtrasadosQueryHandler : IRequestHandler<GetEnviosAtrasadosQuery, ErrorOr<List<EnvioAtrasadoResponse>>>
    {
        private readonly IEnvioRepository _envioRepository;
        private readonly IFestivoRepository _festivoRepository;
        private readonly ITipoServicioRepository _tipoServicioRepository;

        public GetEnviosAtrasadosQueryHandler(
            IEnvioRepository envioRepository,
            IFestivoRepository festivoRepository,
            ITipoServicioRepository tipoServicioRepository)
        {
            _envioRepository = envioRepository ?? throw new ArgumentNullException(nameof(envioRepository));
            _festivoRepository = festivoRepository ?? throw new ArgumentNullException(nameof(festivoRepository));
            _tipoServicioRepository = tipoServicioRepository ?? throw new ArgumentNullException(nameof(tipoServicioRepository));
        }

        public async Task<ErrorOr<List<EnvioAtrasadoResponse>>> Handle(GetEnviosAtrasadosQuery request, CancellationToken cancellationToken)
        {
            if (request.Hasta < request.Desde)
            {
                return EnvioErrors.RangoFechasInvalido;
            }

            var festivos = (await _festivoRepository.GetAllAsync()).Select(f => f.Fecha).ToHashSet();
            var slaPorTipo = (await _tipoServicioRepository.GetAll()).ToDictionary(t => t.Id, t => t.DiasSla);
            var candidatos = await _envioRepository.GetCandidatosAtrasoAsync(request.Desde, request.Hasta);

            var ahora = DateTime.Now;
            var atrasados = new List<EnvioAtrasadoResponse>();

            foreach (var envio in candidatos)
            {
                if (!slaPorTipo.TryGetValue(envio.TipoServicioId, out var diasSla) || diasSla is null)
                {
                    continue;
                }

                var transcurridos = DiasHabiles.Transcurridos(envio.FechaCreacion, ahora, festivos);

                if (envio.EstaAtrasado(diasSla.Value, transcurridos))
                {
                    atrasados.Add(new EnvioAtrasadoResponse(
                        envio.Id,
                        envio.CodigoRastreo,
                        envio.Estado,
                        envio.TipoServicioId,
                        envio.FechaCreacion,
                        diasSla.Value,
                        transcurridos,
                        transcurridos - diasSla.Value));
                }
            }

            return atrasados;
        }
    }
}
