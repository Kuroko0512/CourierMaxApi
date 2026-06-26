using Aplicacion.Envio.Common;
using AutoMapper;
using Dominio.V1.Envio;
using ErrorOr;
using MediatR;

namespace Aplicacion.Envio.GetAll
{
    public class GetAllEnviosQueryHandler : IRequestHandler<GetAllEnviosQuery, ErrorOr<List<EnvioResponse>>>
    {
        private readonly IEnvioRepository _envioRepository;
        private readonly IMapper _mapper;

        public GetAllEnviosQueryHandler(IEnvioRepository envioRepository, IMapper mapper)
        {
            _envioRepository = envioRepository ?? throw new ArgumentNullException(nameof(envioRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ErrorOr<List<EnvioResponse>>> Handle(GetAllEnviosQuery request, CancellationToken cancellationToken)
        {
            var envios = await _envioRepository.GetAll();

            return _mapper.Map<List<EnvioResponse>>(envios);
        }
    }
}
