using Dominio.Primitives;

namespace Dominio.V1.Envio
{
    public record EnvioCreadoDomainEvent(Guid id, string CodigoRastreo) : DomainEvent(id);
}
