using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using SagaImpl.Common;
using SagaImpl.Common.Abstraction.Interface;
using SagaImpl.Common.RabbitMQ.Abstraction;
using SagaImpl.InventoryService.HostingServices;
using SagaImpl.InventoryService.Messaging.Receiver;

namespace SagaImpl.InventoryService.Extensions
{
    public static class ConfigureExtensions
    {
        public static IServiceCollection RegisterInventoryRabbitMqChannels(this IServiceCollection services)
        {
            services.AddSingleton(s =>
            {
                var connectionProvider = s.GetService<IConnectionProvider>();
                var logger = s.GetService<ILoggerAdapter<ReserveItemSubscriber>>();

                return new ReserveItemSubscriber(connectionProvider, logger, CommonConstants.ORDER_SERVICE_EXCHANGE, CommonConstants.RESERVE_ITEMS_COMMAND, ExchangeType.Topic);
            });

            services.AddHostedService<ReserveItemListener>();

            return services;
        }
    }
}
