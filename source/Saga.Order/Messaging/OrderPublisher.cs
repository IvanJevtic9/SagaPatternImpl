using Saga.Common.RabbitMQ;
using Saga.Common.RabbitMQ.Abstraction;

namespace Saga.Order.Messaging
{
    public class OrderPublisher : Publisher
    {
        public OrderPublisher(
            IConnectionProvider connectionProvider,
            string exchangeName,
            string exchangeType,
            int timeToLive = 30000)
            : base(connectionProvider, exchangeName, exchangeType, timeToLive)
        {
        }
    }
}
