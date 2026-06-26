using Dominio.V1.Vehiculo;
using Microsoft.EntityFrameworkCore;

namespace Arquitectura.Persistencia.Repositories
{
    public class VehiculoRepository : IVehiculoRepository
    {
        private readonly ApplicationDbContext _context;

        public VehiculoRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<VehiculoD?> GetByIdAsync(int id)
            => await _context.vehiculoDs.FirstOrDefaultAsync(v => v.Id == id);
    }
}
