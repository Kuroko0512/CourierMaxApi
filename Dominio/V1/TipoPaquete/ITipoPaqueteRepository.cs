namespace Dominio.V1.TipoPaquete
{
    public interface ITipoPaqueteRepository
    {
        Task<List<TipoPaqueteD>> GetAll();

        Task<TipoPaqueteD?> GetByIdAsync(int id);
    }
}
