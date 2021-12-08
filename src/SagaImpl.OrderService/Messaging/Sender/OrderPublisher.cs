using SagaImpl.Common.Abstraction.Interface;
using SagaImpl.Common.RabbitMQ.Abstraction;

namespace SagaImpl.Common.RabbitMQ.Sender
{
    public class OrderPublisher : Publisher<OrderPublisher>
    {
        public OrderPublisher(
            IConnectionProvider connectionProvider,
            ILoggerAdapter<OrderPublisher> logger,
            string exchangeName,
            string exchangeType,
            int timeToLive = 30000) 
        : base(connectionProvider, logger, exchangeName,exchangeType, timeToLive)
        { }
    }
}