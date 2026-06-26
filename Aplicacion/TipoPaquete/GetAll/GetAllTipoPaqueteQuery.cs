using Aplicacion.TipoPaquete.Common;
using ErrorOr;
using MediatR;

namespace Aplicacion.TipoPaquete.GetAll
{
    public record GetAllTipoPaqueteQuery() : IRequest<ErrorOr<List<TipoPaqueteResponse>>>;
}
