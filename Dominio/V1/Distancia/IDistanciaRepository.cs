namespace Dominio.V1.Distancia
{
    public interface IDistanciaRepository
    {
        Task<DistanciaD?> GetByParAsync(int ciudadOrigenId, int ciudadDestinoId);
    }
}
