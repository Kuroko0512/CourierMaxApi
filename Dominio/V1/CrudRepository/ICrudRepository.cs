namespace Dominio.V1.CrudRepository
{
    public interface ICrudRepository<T, TKey>
    {
        void Add(T item);

        void Update(T item);

        void Delete(T guid);

        Task<T?> GetByIdAsync(TKey id);

        Task<List<T>> GetAll();
    }
}
