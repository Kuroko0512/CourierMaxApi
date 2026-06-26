namespace Dominio.Primitives
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangeAsync(CancellationToken cancellationToken = default);
    }
}
