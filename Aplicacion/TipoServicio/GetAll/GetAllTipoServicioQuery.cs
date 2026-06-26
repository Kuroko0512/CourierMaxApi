using Aplicacion.TipoServicio.Common;
using ErrorOr;
using MediatR;

namespace Aplicacion.TipoServicio.GetAll
{
    public record GetAllTipoServicioQuery() : IRequest<ErrorOr<List<TipoServicioResponse>>>;
}
