using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using SagaImpl.Common.Abstraction.Interface;
using SagaImpl.Common.RabbitMQ.Abstraction;
using SagaImpl.Common.RabbitMQ.Sender;

namespace SagaImpl.OrderService.Extensions
{
    public static class ConfigureExtensions
    {
        public static IServiceCollection RegisterOrderPublishBus(this IServiceCollection services)
        {
            services.AddTransient(s =>
            {
                var connectionProvider = s.GetService<IConnectionProvider>();
                var logger = s.GetService<ILoggerAdapter<OrderPublisher>>();

                return new OrderPublisher(connectionProvider, logger, ExchangeType.Direct);
            });

            return services;
        }
    }
}
