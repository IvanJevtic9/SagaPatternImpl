using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using SagaImpl.Common;
using SagaImpl.Common.Abstraction.Interface;
using SagaImpl.Common.RabbitMQ.Abstraction;
using SagaImpl.Common.RabbitMQ.Sender;
using SagaImpl.OrderService.Messaging.Receiver;

namespace SagaImpl.OrderService.Extensions
{
    public static class ConfigureExtensions
    {
        public static IServiceCollection RegisterOrderRabbitMqChannels(this IServiceCollection services)
        {
            services.AddSingleton(s =>
            {
                var connectionProvider = s.GetService<IConnectionProvider>();
                var logger = s.GetService<ILoggerAdapter<OrderPublisher>>();

                return new OrderPublisher(connectionProvider, logger, CommonConstants.ORDER_SERVICE_EXCHANGE, ExchangeType.Topic);
            });

            services.AddSingleton(s =>
            {
                var connectionProvider = s.GetService<IConnectionProvider>();
                var logger = s.GetService<ILoggerAdapter<OrchestratorSubscriber>>();

                return new OrchestratorSubscriber(connectionProvider, logger, CommonConstants.CREATE_ORDER_ORCHESTRATION, CommonConstants.CREATE_ORDER_ORCHESTRATION, ExchangeType.Topic);
            });

            return services;
        }
    }
}
