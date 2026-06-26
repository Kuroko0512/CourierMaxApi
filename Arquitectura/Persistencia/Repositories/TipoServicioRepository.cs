using Dominio.V1.TipoServicio;
using Microsoft.EntityFrameworkCore;

namespace Arquitectura.Persistencia.Repositories
{
    public class TipoServicioRepository : ITipoServicioRepository
    {
        private readonly ApplicationDbContext _context;

        public TipoServicioRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<TipoServicioD>> GetAll()
            => await _context.tipoServicioDs.Where(t => t.Activo).OrderBy(t => t.Orden).ToListAsync();

        public async Task<TipoServicioD?> GetByIdAsync(int id)
            => await _context.tipoServicioDs.FirstOrDefaultAsync(t => t.Id == id && t.Activo);
    }
}
