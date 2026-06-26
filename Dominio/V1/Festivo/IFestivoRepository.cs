namespace Dominio.V1.Festivo
{
    public interface IFestivoRepository
    {
        Task<List<FestivoD>> GetAllAsync();
    }
}
