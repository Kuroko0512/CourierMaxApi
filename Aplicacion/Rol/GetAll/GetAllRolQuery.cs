using Aplicacion.Rol.Common;
using ErrorOr;
using MediatR;

namespace Aplicacion.Rol.GetAll
{
    public record GetAllRolQuery() : IRequest<ErrorOr<List<RolResponse>>>;
}
