using SagaImpl.Common.Abstraction.Interface;
using SagaImpl.Common.RabbitMQ;
using SagaImpl.Common.RabbitMQ.Abstraction;

namespace SagaImpl.InventoryService.Messaging.Sender
{
    public class InventoryPublisher : Publisher<InventoryPublisher>
    {
        public InventoryPublisher(IConnectionProvider connectionProvider, ILoggerAdapter<InventoryPublisher> logger, string exchangeName, string exchangeType, int timeToLive = 30000)
            : base(connectionProvider, logger, exchangeName, exchangeType, timeToLive)
        {
        }
    }
}
