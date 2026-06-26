using Aplicacion.Auth;
using Aplicacion.Envio.Common;
using AutoMapper;
using Dominio.Primitives;
using Dominio.V1.Conductor;
using Dominio.V1.Envio;
using Dominio.V1.Vehiculo;
using ErrorOr;
using MediatR;

namespace Aplicacion.Envio.CambiarEstado
{
    public class CambiarEstadoCommandHandler : IRequestHandler<CambiarEstadoCommand, ErrorOr<EnvioResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEnvioRepository _envioRepository;
        private readonly IConductorRepository _conductorRepository;
        private readonly IVehiculoRepository _vehiculoRepository;
        private readonly ICurrentUser _currentUser;

        public CambiarEstadoCommandHandler(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IEnvioRepository envioRepository,
            IConductorRepository conductorRepository,
            IVehiculoRepository vehiculoRepository,
            ICurrentUser currentUser)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _envioRepository = envioRepository ?? throw new ArgumentNullException(nameof(envioRepository));
            _conductorRepository = conductorRepository ?? throw new ArgumentNullException(nameof(conductorRepository));
            _vehiculoRepository = vehiculoRepository ?? throw new ArgumentNullException(nameof(vehiculoRepository));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        public async Task<ErrorOr<EnvioResponse>> Handle(CambiarEstadoCommand command, CancellationToken cancellationToken)
        {
            var envio = await _envioRepository.GetByIdAsync(command.Id);
            if (envio is null)
            {
                return EnvioErrors.NoEncontrado;
            }

            int? conductorAsignado = null;

            if (command.NuevoEstado == EstadoEnvio.ASIGNADO && envio.Estado == EstadoEnvio.CREADO)
            {
                var seleccion = await SeleccionarConductorAsync(envio);
                if (seleccion.IsError)
                {
                    return seleccion.Errors;
                }

                conductorAsignado = seleccion.Value;
            }

            var resultado = envio.CambiarEstado(command.NuevoEstado, command.Motivo, _currentUser.UsuarioId, conductorAsignado);
            if (resultado.IsError)
            {
                return resultado.Errors;
            }

            await _unitOfWork.SaveChangeAsync(cancellationToken);

            return _mapper.Map<EnvioResponse>(envio);
        }

        private async Task<ErrorOr<int>> SeleccionarConductorAsync(EnvioD envio)
        {
            var conductores = await _conductorRepository.GetActivosAsync();

            int? elegido = null;
            var menorVolumen = decimal.MaxValue;
            var menorPeso = decimal.MaxValue;

            foreach (var conductor in conductores)
            {
                var vehiculo = await _vehiculoRepository.GetByIdAsync(conductor.VehiculoId);
                if (vehiculo is null)
                {
                    continue;
                }

                var (peso, volumen) = await _envioRepository.GetCargaAsignadaAsync(conductor.Id);

                if (!vehiculo.TieneCapacidad(peso + envio.Paquete.PesoKg, volumen + envio.Paquete.VolumenM3))
                {
                    continue;
                }

                if (volumen < menorVolumen || (volumen == menorVolumen && peso < menorPeso))
                {
                    menorVolumen = volumen;
                    menorPeso = peso;
                    elegido = conductor.Id;
                }
            }

            if (elegido is null)
            {
                return EnvioErrors.SinVehiculosDisponibles;
            }

            return elegido.Value;
        }
    }
}
