using Dominio.V1.Envio;
using Microsoft.EntityFrameworkCore;

namespace Arquitectura.Persistencia.Repositories
{
    public class EnvioRepository : IEnvioRepository
    {
        private readonly ApplicationDbContext _context;

        public EnvioRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Add(EnvioD item)
            => _context.enviosDs.Add(item);

        public void Delete(EnvioD item)
            => _context.enviosDs.Remove(item);

        public void Update(EnvioD item)
            => _context.enviosDs.Update(item);

        public async Task<List<EnvioD>> GetAll()
            => await _context.enviosDs.Include(e => e.Historial).ToListAsync();

        public async Task<EnvioD?> GetByIdAsync(int id)
            => await _context.enviosDs.Include(e => e.Historial).FirstOrDefaultAsync(e => e.Id == id);

        public async Task<EnvioD?> GetByCodigoAsync(string codigoRastreo)
            => await _context.enviosDs.Include(e => e.Historial).FirstOrDefaultAsync(e => e.CodigoRastreo == codigoRastreo);

        public async Task<(decimal PesoKg, decimal VolumenM3)> GetCargaAsignadaAsync(int conductorId)
        {
            var cargas = await _context.enviosDs
                .Where(e => e.ConductorId == conductorId
                    && (e.Estado == EstadoEnvio.ASIGNADO || e.Estado == EstadoEnvio.EN_TRANSITO))
                .Select(e => new { e.Paquete.PesoKg, e.Paquete.VolumenM3 })
                .ToListAsync();

            return (cargas.Sum(c => c.PesoKg), cargas.Sum(c => c.VolumenM3));
        }

        public async Task<List<EnvioD>> GetCandidatosAtrasoAsync(DateTime desde, DateTime hasta)
            => await _context.enviosDs
                .Where(e => e.FechaCreacion >= desde && e.FechaCreacion <= hasta
                    && e.Estado != EstadoEnvio.ENTREGADO && e.Estado != EstadoEnvio.CANCELADO)
                .ToListAsync();

        public async Task<List<EnvioD>> GetAsignadosAsync(DateTime? desde, DateTime? hasta)
        {
            var query = _context.enviosDs.Where(e => e.ConductorId != null);

            if (desde.HasValue)
            {
                query = query.Where(e => e.FechaAsignacion >= desde.Value);
            }

            if (hasta.HasValue)
            {
                query = query.Where(e => e.FechaAsignacion <= hasta.Value);
            }

            return await query.ToListAsync();
        }
    }
}
