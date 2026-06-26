namespace Dominio.V1.Usuario
{
    public interface IUsuarioRepository
    {
        Task<UsuarioD?> GetByNombreAsync(string nombre);
    }
}
