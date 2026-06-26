namespace Dominio.V1.TipoServicio
{
    public interface ITipoServicioRepository
    {
        Task<List<TipoServicioD>> GetAll();

        Task<TipoServicioD?> GetByIdAsync(int id);
    }
}
