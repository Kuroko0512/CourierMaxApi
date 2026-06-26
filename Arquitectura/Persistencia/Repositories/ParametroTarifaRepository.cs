using Dominio.V1.ParametroTarifa;
using Microsoft.EntityFrameworkCore;

namespace Arquitectura.Persistencia.Repositories
{
    public class ParametroTarifaRepository : IParametroTarifaRepository
    {
        private readonly ApplicationDbContext _context;

        public ParametroTarifaRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<ParametroTarifaD?> GetAsync()
            => await _context.parametroTarifaDs.FirstOrDefaultAsync();
    }
}
