using Aplicacion.Rol.Common;
using AutoMapper;
using Dominio.V1.Rol;
using ErrorOr;
using MediatR;

namespace Aplicacion.Rol.GetAll
{
    public class GetAllRolQueryHandler : IRequestHandler<GetAllRolQuery, ErrorOr<List<RolResponse>>>
    {
        private readonly IRolRepository _repository;
        private readonly IMapper _mapper;

        public GetAllRolQueryHandler(IRolRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ErrorOr<List<RolResponse>>> Handle(GetAllRolQuery request, CancellationToken cancellationToken)
        {
            var roles = await _repository.GetAll();

            return _mapper.Map<List<RolResponse>>(roles);
        }
    }
}
