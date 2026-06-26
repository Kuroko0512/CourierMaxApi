namespace Dominio.Primitives
{
    public abstract class AggregateRoot
    {
        private readonly List<DomainEvent> _domainevent = new();

        public ICollection<DomainEvent> GetDomainEvents() => _domainevent;

        protected void Raise(DomainEvent domainEvent)
        {
            _domainevent.Add(domainEvent);
        }
    }
}
