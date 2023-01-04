using Saga.Common.RabbitMQ;
using Saga.Common.RabbitMQ.Abstraction;

namespace Saga.Inventory.Messaging
{
    public class InventoryPublisher : Publisher
    {
        public InventoryPublisher(
            IConnectionProvider connectionProvider,
            string exchangeName,
            string exchangeType,
            int timeToLive = 30000)
            : base(connectionProvider, exchangeName, exchangeType, timeToLive)
        {
        }
    }
}
