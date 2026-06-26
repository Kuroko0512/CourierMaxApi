using Aplicacion.Envio.Common;
using AutoMapper;
using Dominio.V1.Envio;
using ErrorOr;
using MediatR;

namespace Aplicacion.Envio.GetByCodigo
{
    public class GetByCodigoQueryHandler : IRequestHandler<GetByCodigoQuery, ErrorOr<EnvioResponse>>
    {
        private readonly IEnvioRepository _envioRepository;
        private readonly IMapper _mapper;

        public GetByCodigoQueryHandler(IEnvioRepository envioRepository, IMapper mapper)
        {
            _envioRepository = envioRepository ?? throw new ArgumentNullException(nameof(envioRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ErrorOr<EnvioResponse>> Handle(GetByCodigoQuery request, CancellationToken cancellationToken)
        {
            var envio = await _envioRepository.GetByCodigoAsync(request.CodigoRastreo);
            if (envio is null)
            {
                return EnvioErrors.NoEncontrado;
            }

            return _mapper.Map<EnvioResponse>(envio);
        }
    }
}
