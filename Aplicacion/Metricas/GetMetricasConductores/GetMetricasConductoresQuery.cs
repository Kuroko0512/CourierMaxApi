using Aplicacion.Metricas.Common;
using ErrorOr;
using MediatR;

namespace Aplicacion.Metricas.GetMetricasConductores
{
    public record GetMetricasConductoresQuery(DateTime? Desde, DateTime? Hasta) : IRequest<ErrorOr<List<MetricaConductorResponse>>>;
}
