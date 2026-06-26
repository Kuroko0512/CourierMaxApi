using MediatR;

namespace Dominio.Primitives
{
    public record DomainEvent(Guid id) : INotification;
}
