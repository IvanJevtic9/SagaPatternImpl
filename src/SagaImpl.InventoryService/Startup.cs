using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SagaImpl.Common.Extension;
using SagaImpl.Common.Middelware;
using SagaImpl.Common.Settings;
using SagaImpl.InventoryService.Database;
using SagaImpl.InventoryService.Extensions;

namespace SagaImpl.InventoryService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.MapConfigurationSettings(Configuration);
            services.AddDatabaseContext<InventoryDbContext>(Configuration.GetSettings<AppSettings>().ConnectionStrings.DefaultConnection);
            services.RegisterServices(Configuration);
            services.RegisterMicroServiceServices();

            services.AddMediatrConfiguration(typeof(UnitOfWork));
            services.AddRabbitMQConnectionProvider();
            services.RegisterInventoryRabbitMqChannels();

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, SwaggerSettings swaggerSettings)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ExceptionMiddelware>();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(swaggerSettings.JsonRoute, $"{swaggerSettings.Title} {swaggerSettings.Version}");
            });

            app.UseRouting();

            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
