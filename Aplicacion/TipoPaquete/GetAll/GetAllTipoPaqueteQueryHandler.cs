using Aplicacion.TipoPaquete.Common;
using AutoMapper;
using Dominio.V1.TipoPaquete;
using ErrorOr;
using MediatR;

namespace Aplicacion.TipoPaquete.GetAll
{
    public class GetAllTipoPaqueteQueryHandler : IRequestHandler<GetAllTipoPaqueteQuery, ErrorOr<List<TipoPaqueteResponse>>>
    {
        private readonly ITipoPaqueteRepository _repository;
        private readonly IMapper _mapper;

        public GetAllTipoPaqueteQueryHandler(ITipoPaqueteRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ErrorOr<List<TipoPaqueteResponse>>> Handle(GetAllTipoPaqueteQuery request, CancellationToken cancellationToken)
        {
            var tipos = await _repository.GetAll();

            return _mapper.Map<List<TipoPaqueteResponse>>(tipos);
        }
    }
}
