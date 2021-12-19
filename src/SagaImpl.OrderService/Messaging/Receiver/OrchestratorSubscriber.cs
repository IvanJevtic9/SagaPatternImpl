using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SagaImpl.Common.Abstraction.Interface;
using SagaImpl.Common.Extension;
using SagaImpl.Common.RabbitMQ;
using SagaImpl.Common.RabbitMQ.Abstraction;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SagaImpl.OrderService.Messaging.Receiver
{
    public class OrchestratorSubscriber : Subscriber<OrchestratorSubscriber>
    {
        private readonly EventingBasicConsumer consumer;
        public OrchestratorSubscriber(
            IConnectionProvider connectionProvider,
            ILoggerAdapter<OrchestratorSubscriber> logger,
            string exchangeName,
            string queueName,
            string exchangeType,
            int timeToLive = 30000)
        : base(connectionProvider, logger, queueName, exchangeName, exchangeType, timeToLive)
        {
            consumer = new EventingBasicConsumer(channel);
            channel.BasicConsume(queueName, false, consumer);
        }

        public override void SubscribeAsync(Func<string, IDictionary<string, object>, Task<bool>> callback)
        {
            consumer.Received += async (sender, e) =>
            {
                var body = e.Body.ToArray();
                var message = body.GetString();

                bool success = await callback.Invoke(message, e.BasicProperties.Headers);

                logger.LogInformation($"Exchange: {exchangeName} received rbmq message. Routing key: {queueName}");

                if (success) channel.BasicAck(e.DeliveryTag, false);
            };
        }
    }
}