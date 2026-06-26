using Dominio.V1.Rol;
using Microsoft.EntityFrameworkCore;

namespace Arquitectura.Persistencia.Repositories
{
    public class RolRepository : IRolRepository
    {
        private readonly ApplicationDbContext _context;

        public RolRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<RolD>> GetAll()
            => await _context.rolDs.Where(t => t.Activo).OrderBy(t => t.Orden).ToListAsync();

        public async Task<bool> ExistsAsync(int id)
            => await _context.rolDs.AnyAsync(t => t.Id == id && t.Activo);
    }
}
