using Saga.Common.RabbitMQ;
using Saga.Common.RabbitMQ.Abstraction;

namespace Saga.Order.Messaging
{
    public class InventorySubscriber : Subscriber
    {
        public InventorySubscriber(
            IConnectionProvider connectionProvider,
            string exchangeName,
            string queueName,
            string exchangeType,
            int timeToLive = 30000)
        : base(connectionProvider, queueName, exchangeName, exchangeType, timeToLive)
        { }
    }
}