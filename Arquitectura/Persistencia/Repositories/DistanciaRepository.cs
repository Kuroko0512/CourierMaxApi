using Dominio.V1.Distancia;
using Microsoft.EntityFrameworkCore;

namespace Arquitectura.Persistencia.Repositories
{
    public class DistanciaRepository : IDistanciaRepository
    {
        private readonly ApplicationDbContext _context;

        public DistanciaRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<DistanciaD?> GetByParAsync(int ciudadOrigenId, int ciudadDestinoId)
            => await _context.distanciaDs.FirstOrDefaultAsync(d =>
                d.CiudadOrigenId == ciudadOrigenId && d.CiudadDestinoId == ciudadDestinoId);
    }
}
