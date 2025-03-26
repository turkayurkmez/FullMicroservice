using ECommerce.Common.Behaviors;
using ECommerce.Common.Behaviours;
using FluentValidation;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ECommerce.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCommonServices(this IServiceCollection services, Assembly assembly)
        {
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(assembly);

                config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
                config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));


            });

            services.AddValidatorsFromAssembly(assembly);

            var typedAdapterConfig = TypeAdapterConfig.GlobalSettings;
            typedAdapterConfig.Scan(assembly);
            services.AddSingleton(typedAdapterConfig);
            services.AddScoped<IMapper, ServiceMapper>();

            return services;
        }
    }
}
