namespace Dominio.V1.Conductor
{
    public interface IConductorRepository
    {
        Task<ConductorD?> GetByIdAsync(int id);

        Task<List<ConductorD>> GetActivosAsync();
    }
}
