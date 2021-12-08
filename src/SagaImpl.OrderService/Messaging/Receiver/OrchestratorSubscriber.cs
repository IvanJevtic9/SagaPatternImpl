using SagaImpl.Common.Abstraction.Interface;
using SagaImpl.Common.RabbitMQ;
using SagaImpl.Common.RabbitMQ.Abstraction;

namespace SagaImpl.OrderService.Messaging.Receiver
{
    public class OrchestratorSubscriber : Subscriber<OrchestratorSubscriber>
    {
        public OrchestratorSubscriber(
            IConnectionProvider connectionProvider,
            ILoggerAdapter<OrchestratorSubscriber> logger,
            string exchangeName,
            string queueName,
            string exchangeType,
            int timeToLive = 30000)
        : base(connectionProvider, logger, queueName, exchangeName, exchangeType, timeToLive)
        { }
    }
}