using Dominio.V1.Conductor;
using Microsoft.EntityFrameworkCore;

namespace Arquitectura.Persistencia.Repositories
{
    public class ConductorRepository : IConductorRepository
    {
        private readonly ApplicationDbContext _context;

        public ConductorRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<ConductorD?> GetByIdAsync(int id)
            => await _context.conductorDs.FirstOrDefaultAsync(c => c.Id == id);

        public async Task<List<ConductorD>> GetActivosAsync()
            => await _context.conductorDs.Where(c => c.Activo).OrderBy(c => c.Id).ToListAsync();
    }
}
