using Saga.Common.RabbitMQ;
using Saga.Common.RabbitMQ.Abstraction;

namespace Saga.Inventory.Messaging
{
    public class OrderSubscriber : Subscriber
    {
        public OrderSubscriber(
            IConnectionProvider connectionProvider,
            string exchangeName,
            string queueName,
            string exchangeType,
            int timeToLive = 30000)
        : base(connectionProvider, queueName, exchangeName, exchangeType, timeToLive)
        { }
    }
}