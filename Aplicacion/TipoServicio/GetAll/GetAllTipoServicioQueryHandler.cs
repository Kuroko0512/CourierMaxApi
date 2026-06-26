using Aplicacion.TipoServicio.Common;
using AutoMapper;
using Dominio.V1.TipoServicio;
using ErrorOr;
using MediatR;

namespace Aplicacion.TipoServicio.GetAll
{
    public class GetAllTipoServicioQueryHandler : IRequestHandler<GetAllTipoServicioQuery, ErrorOr<List<TipoServicioResponse>>>
    {
        private readonly ITipoServicioRepository _repository;
        private readonly IMapper _mapper;

        public GetAllTipoServicioQueryHandler(ITipoServicioRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ErrorOr<List<TipoServicioResponse>>> Handle(GetAllTipoServicioQuery request, CancellationToken cancellationToken)
        {
            var tipos = await _repository.GetAll();

            return _mapper.Map<List<TipoServicioResponse>>(tipos);
        }
    }
}
