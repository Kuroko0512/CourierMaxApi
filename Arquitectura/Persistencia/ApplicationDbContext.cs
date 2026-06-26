using Aplicacion.Data;
using Dominio.Primitives;
using Dominio.V1.Conductor;
using Dominio.V1.Distancia;
using Dominio.V1.Envio;
using Dominio.V1.Festivo;
using Dominio.V1.ParametroTarifa;
using Dominio.V1.Rol;
using Dominio.V1.TipoPaquete;
using Dominio.V1.TipoServicio;
using Dominio.V1.Usuario;
using Dominio.V1.Vehiculo;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Arquitectura.Persistencia
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext, IUnitOfWork
    {

        private readonly IPublisher _publisher;


        #region DBSETS
        public DbSet<EnvioD> enviosDs { get; set; }

        public DbSet<TipoServicioD> tipoServicioDs { get; set; }

        public DbSet<TipoPaqueteD> tipoPaqueteDs { get; set; }

        public DbSet<RolD> rolDs { get; set; }

        public DbSet<DistanciaD> distanciaDs { get; set; }

        public DbSet<ParametroTarifaD> parametroTarifaDs { get; set; }

        public DbSet<ConductorD> conductorDs { get; set; }

        public DbSet<VehiculoD> vehiculoDs { get; set; }

        public DbSet<UsuarioD> usuarioDs { get; set; }

        public DbSet<FestivoD> festivoDs { get; set; }

        #endregion

        public ApplicationDbContext(DbContextOptions options, IPublisher publisher) : base(options)
        {
            _publisher = publisher ?? throw new ArgumentException(nameof(publisher));
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

        public async Task<int> SaveChangeAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var domainevent = ChangeTracker.Entries<AggregateRoot>()
                .Select(e => e.Entity)
                .Where(e => e.GetDomainEvents().Any())
                .SelectMany(e => e.GetDomainEvents());

            var result = await base.SaveChangesAsync(cancellationToken);

            foreach (var domainEvent in domainevent)
            {
                await _publisher.Publish(domainEvent, cancellationToken);
            }

            return result;
        }
    }
}
