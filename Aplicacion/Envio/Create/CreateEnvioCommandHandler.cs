using Aplicacion.Envio.Common;
using AutoMapper;
using Dominio.Primitives;
using Dominio.V1.Distancia;
using Dominio.V1.Envio;
using Dominio.V1.Envio.ValueObjects;
using Dominio.V1.ParametroTarifa;
using Dominio.V1.TipoPaquete;
using Dominio.V1.TipoServicio;
using ErrorOr;
using MediatR;

namespace Aplicacion.Envio.Create
{
    public class CreateEnvioCommandHandler : IRequestHandler<CreateEnvioCommand, ErrorOr<EnvioResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEnvioRepository _envioRepository;
        private readonly ITipoServicioRepository _tipoServicioRepository;
        private readonly ITipoPaqueteRepository _tipoPaqueteRepository;
        private readonly IDistanciaRepository _distanciaRepository;
        private readonly IParametroTarifaRepository _parametroTarifaRepository;

        public CreateEnvioCommandHandler(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IEnvioRepository envioRepository,
            ITipoServicioRepository tipoServicioRepository,
            ITipoPaqueteRepository tipoPaqueteRepository,
            IDistanciaRepository distanciaRepository,
            IParametroTarifaRepository parametroTarifaRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _envioRepository = envioRepository ?? throw new ArgumentNullException(nameof(envioRepository));
            _tipoServicioRepository = tipoServicioRepository ?? throw new ArgumentNullException(nameof(tipoServicioRepository));
            _tipoPaqueteRepository = tipoPaqueteRepository ?? throw new ArgumentNullException(nameof(tipoPaqueteRepository));
            _distanciaRepository = distanciaRepository ?? throw new ArgumentNullException(nameof(distanciaRepository));
            _parametroTarifaRepository = parametroTarifaRepository ?? throw new ArgumentNullException(nameof(parametroTarifaRepository));
        }

        public async Task<ErrorOr<EnvioResponse>> Handle(CreateEnvioCommand command, CancellationToken cancellationToken)
        {
            var tipoServicio = await _tipoServicioRepository.GetByIdAsync(command.TipoServicioId);
            if (tipoServicio is null)
            {
                return EnvioErrors.TipoServicioInvalido;
            }

            var tipoPaquete = await _tipoPaqueteRepository.GetByIdAsync(command.TipoPaqueteId);
            if (tipoPaquete is null)
            {
                return EnvioErrors.TipoPaqueteInvalido;
            }

            var distancia = await _distanciaRepository.GetByParAsync(command.CiudadOrigenId, command.CiudadDestinoId);
            if (distancia is null)
            {
                return EnvioErrors.RutaSinTarifa;
            }

            var parametro = await _parametroTarifaRepository.GetAsync();
            if (parametro is null)
            {
                return EnvioErrors.ParametroTarifaNoConfigurado;
            }

            var remitente = new Remitente(command.RemitenteNombre, command.RemitenteTelefono, command.RemitenteDireccionRecogida);
            var destinatario = new Destinatario(command.DestinatarioNombre, command.DestinatarioTelefono, command.DestinatarioDireccionEntrega);
            var paquete = new Paquete(command.PesoKg, command.LargoCm, command.AnchoCm, command.AltoCm, command.TipoPaqueteId);

            var envio = EnvioD.Crear(command.TipoServicioId, remitente, destinatario, paquete, command.CiudadOrigenId, command.CiudadDestinoId);

            envio.CalcularCosto(
                tipoServicio.TarifaBase,
                distancia.TarifaDistancia,
                tipoPaquete.RecargoPorcentaje,
                parametro.PesoBaseKg,
                parametro.RecargoPorKgAdicional);

            _envioRepository.Add(envio);
            await _unitOfWork.SaveChangeAsync(cancellationToken);

            return _mapper.Map<EnvioResponse>(envio);
        }
    }
}
