using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using SagaImpl.Common;
using SagaImpl.Common.Abstraction.Interface;
using SagaImpl.Common.RabbitMQ.Abstraction;
using SagaImpl.InventoryService.Database;
using SagaImpl.InventoryService.HostingServices;
using SagaImpl.InventoryService.Messaging.Receiver;
using SagaImpl.InventoryService.Messaging.Sender;

namespace SagaImpl.InventoryService.Extensions
{
    public static class ConfigureExtensions
    {
        public static IServiceCollection RegisterInventoryRabbitMqChannels(this IServiceCollection services)
        {
            services.AddScoped(s =>
            {
                var connectionProvider = s.GetService<IConnectionProvider>();
                var logger = s.GetService<ILoggerAdapter<ReserveItemSubscriber>>();

                return new ReserveItemSubscriber(connectionProvider, logger, CommonConstants.ORDER_SERVICE_EXCHANGE, CommonConstants.RESERVE_ITEMS_COMMAND, ExchangeType.Topic);
            });

            services.AddScoped(s =>
            {
                var connectionProvider = s.GetService<IConnectionProvider>();
                var logger = s.GetService<ILoggerAdapter<InventoryPublisher>>();

                return new InventoryPublisher(connectionProvider, logger, CommonConstants.ORCHESTRATION_EXCHANGE, ExchangeType.Topic);
            });

            services.AddHostedService<ReserveItemListener>();

            return services;
        }

        public static IServiceCollection RegisterMicroServiceServices(this IServiceCollection services)
        {
            services.AddTransient<UnitOfWork>();

            return services;
        }
    }
}
