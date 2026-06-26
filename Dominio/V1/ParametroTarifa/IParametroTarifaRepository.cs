namespace Dominio.V1.ParametroTarifa
{
    public interface IParametroTarifaRepository
    {
        Task<ParametroTarifaD?> GetAsync();
    }
}
