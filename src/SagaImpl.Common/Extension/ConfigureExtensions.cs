using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using SagaImpl.Common.Abstraction.Interface;
using SagaImpl.Common.Apstraction.Implementation;
using SagaImpl.Common.RabbitMQ;
using SagaImpl.Common.RabbitMQ.Abstraction;
using SagaImpl.Common.Settings;

namespace SagaImpl.Common.Extension
{
    public static class ConfigureExtensions
    {
        public static IServiceCollection AddDatabaseContext<TContext>(this IServiceCollection services, string connectionString) where TContext : DbContext
        {
            return services.AddDbContext<TContext>(b => b.UseSqlServer(connectionString)
                                                         .UseLazyLoadingProxies());
        }

        public static IServiceCollection MapConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettings = configuration.GetSettings<AppSettings>();
            var rabbitMqSettings = configuration.GetSettings<RabbitMQSettings>();

            services.AddOptions();
            services.AddSingleton(appSettings);
            services.AddSingleton(rabbitMqSettings);

            return services;
        }

        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            var swaggerSettings = configuration.GetSettings<SwaggerSettings>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(swaggerSettings.Version, new OpenApiInfo { Title = swaggerSettings.Title, Version = swaggerSettings.Version });
            });

            services.AddSingleton(swaggerSettings);
            services.AddTransient(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));
            
            return services;
        }

        public static IServiceCollection AddRabbitMQConnectionProvider(this IServiceCollection services)
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
