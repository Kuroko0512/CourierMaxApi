using Dominio.V1.CrudRepository;
namespace Dominio.V1.Envio
{
    public interface IEnvioRepository : ICrudRepository<EnvioD, int>
    {
        Task<EnvioD?> GetByCodigoAsync(string codigoRastreo);

        Task<(decimal PesoKg, decimal VolumenM3)> GetCargaAsignadaAsync(int conductorId);

        Task<List<EnvioD>> GetCandidatosAtrasoAsync(DateTime desde, DateTime hasta);

        Task<List<EnvioD>> GetAsignadosAsync(DateTime? desde, DateTime? hasta);
    }
}
