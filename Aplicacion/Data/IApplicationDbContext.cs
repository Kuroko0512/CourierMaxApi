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
using Microsoft.EntityFrameworkCore;

namespace Aplicacion.Data
{
    public interface IApplicationDbContext
    {
        DbSet<EnvioD> enviosDs { get; set; }

        DbSet<TipoServicioD> tipoServicioDs { get; set; }

        DbSet<TipoPaqueteD> tipoPaqueteDs { get; set; }

        DbSet<RolD> rolDs { get; set; }

        DbSet<DistanciaD> distanciaDs { get; set; }

        DbSet<ParametroTarifaD> parametroTarifaDs { get; set; }

        DbSet<ConductorD> conductorDs { get; set; }

        DbSet<VehiculoD> vehiculoDs { get; set; }

        DbSet<UsuarioD> usuarioDs { get; set; }

        DbSet<FestivoD> festivoDs { get; set; }

        Task<int> SaveChangeAsync(CancellationToken cancellationToken);
    }
}
