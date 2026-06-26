namespace Dominio.V1.Vehiculo
{
    public interface IVehiculoRepository
    {
        Task<VehiculoD?> GetByIdAsync(int id);
    }
}
