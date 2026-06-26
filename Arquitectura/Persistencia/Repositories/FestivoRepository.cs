using Dominio.V1.Festivo;
using Microsoft.EntityFrameworkCore;

namespace Arquitectura.Persistencia.Repositories
{
    public class FestivoRepository : IFestivoRepository
    {
        private readonly ApplicationDbContext _context;

        public FestivoRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<FestivoD>> GetAllAsync()
            => await _context.festivoDs.ToListAsync();
    }
}
