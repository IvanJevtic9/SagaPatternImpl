using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

            services.AddSingleton(appSettings);

            return services;
        }

        public static T GetSettings<T>(this IConfiguration configuration) where T : new()
        {
            var settings = new T();

            configuration.GetSection(typeof(T).Name).Bind(settings);

            return settings;
        }
    }
}
