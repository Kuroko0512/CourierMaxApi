using Dominio.V1.Usuario;
using Microsoft.EntityFrameworkCore;

namespace Arquitectura.Persistencia.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ApplicationDbContext _context;

        public UsuarioRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<UsuarioD?> GetByNombreAsync(string nombre)
            => await _context.usuarioDs.Include(u => u.Rol).FirstOrDefaultAsync(u => u.Nombre == nombre);
    }
}
