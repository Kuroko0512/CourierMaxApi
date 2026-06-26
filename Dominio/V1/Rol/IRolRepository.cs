namespace Dominio.V1.Rol
{
    public interface IRolRepository
    {
        Task<List<RolD>> GetAll();

        Task<bool> ExistsAsync(int id);
    }
}
