using Aplicacion.Auth;
using Aplicacion.Data;
using Arquitectura.Persistencia;
using Arquitectura.Persistencia.Repositories;
using Arquitectura.Services;
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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Arquitectura
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddArquitectura(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddPersistence(configuration);
            return services;
        }

        private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            string? cadena = configuration["Bd"];

            services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(cadena));

            services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());

            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

            #region Scoped con las tablas de la base de datos

            services.AddScoped<IEnvioRepository, EnvioRepository>();

            services.AddScoped<ITipoServicioRepository, TipoServicioRepository>();

            services.AddScoped<ITipoPaqueteRepository, TipoPaqueteRepository>();

            services.AddScoped<IRolRepository, RolRepository>();

            services.AddScoped<IDistanciaRepository, DistanciaRepository>();

            services.AddScoped<IParametroTarifaRepository, ParametroTarifaRepository>();

            services.AddScoped<IConductorRepository, ConductorRepository>();

            services.AddScoped<IVehiculoRepository, VehiculoRepository>();

            services.AddScoped<IUsuarioRepository, UsuarioRepository>();

            services.AddScoped<IFestivoRepository, FestivoRepository>();

            #endregion


            services.AddHttpClient();

            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();


            return services;
        }


    }
}
