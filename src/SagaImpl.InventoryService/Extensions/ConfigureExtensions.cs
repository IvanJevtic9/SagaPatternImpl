using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using SagaImpl.Common;
using SagaImpl.Common.Abstraction.Interface;
using SagaImpl.Common.Apstraction.Implementation;
using SagaImpl.Common.RabbitMQ;
using SagaImpl.Common.RabbitMQ.Abstraction;
using SagaImpl.Common.Settings;
using SagaImpl.InventoryService.Messaging.Receiver;

namespace SagaImpl.InventoryService.Extensions
{
    public static class ConfigureExtensions
    {
        public static IServiceCollection RegisterReceiverBus(this IServiceCollection services)
        {
            // FIX
            services.AddHostedService(s =>
            {
                var logger = new LoggerAdapter<ConnectionProvider>();
                var connectionProvider = new ConnectionProvider(new RabbitMQSettings(), logger);

                var logger1 = new LoggerAdapter<ReserveItemsSubscriber>();
                return new ReserveItemsSubscriber(connectionProvider, logger1, CommonConstants.RESERVE_ITEMS_EVENT, ExchangeType.Direct);
            });

            return services;
        }
    }
}
