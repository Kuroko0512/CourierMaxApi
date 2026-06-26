using Aplicacion.Common.Behaviors;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Aplicacion
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssemblyContaining<ApplicationAssemblyReference>();
                config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

            services.AddValidatorsFromAssemblyContaining<ApplicationAssemblyReference>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());



            return services;
        }

    }
}
