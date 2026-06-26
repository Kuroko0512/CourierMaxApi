using Dominio.V1.TipoPaquete;
using Microsoft.EntityFrameworkCore;

namespace Arquitectura.Persistencia.Repositories
{
    public class TipoPaqueteRepository : ITipoPaqueteRepository
    {
        private readonly ApplicationDbContext _context;

        public TipoPaqueteRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<TipoPaqueteD>> GetAll()
            => await _context.tipoPaqueteDs.Where(t => t.Activo).OrderBy(t => t.Orden).ToListAsync();

        public async Task<TipoPaqueteD?> GetByIdAsync(int id)
            => await _context.tipoPaqueteDs.FirstOrDefaultAsync(t => t.Id == id && t.Activo);
    }
}
