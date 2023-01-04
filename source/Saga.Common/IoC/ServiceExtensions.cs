
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Saga.Common.RabbitMQ;
using Saga.Common.RabbitMQ.Abstraction;
using Saga.Common.Settings;

namespace Saga.Common.IoC
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddDatabaseContext<TContext>(this IServiceCollection services, string connectionString) where TContext : DbContext
        {
            return services.AddDbContext<TContext>(b => b.UseSqlServer(connectionString).UseLazyLoadingProxies(), ServiceLifetime.Transient);
        }

        public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var swagger = configuration.GetSettings<SwaggerSettings>();
            var rabbitMQ = configuration.GetSettings<RabbitMQSettings>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(swagger.Version, new OpenApiInfo 
                { 
                    Title = swagger.Title,
                    Version = swagger.Version
                });
            });

            services.AddSingleton(swagger);
            services.AddSingleton(rabbitMQ);

            return services;
        }

        public static IServiceCollection AddRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddTransient<IConnectionProvider, ConnectionProvider>();
        }

        public static T GetSettings<T>(this IConfiguration configuration) where T : new()
        {
            var settings = new T();

            configuration.GetSection(typeof(T).Name).Bind(settings);

            return settings;
        }
    }
}
