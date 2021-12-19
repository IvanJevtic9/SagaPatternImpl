using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SagaImpl.Common.Abstraction.Interface;
using SagaImpl.Common.RabbitMQ;
using SagaImpl.Common.RabbitMQ.Abstraction;
using SagaImpl.OrderService.Models;
using SagaImpl.OrderService.SagaOrchestration;
using System;

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

        public void SubscribeOnChannel(EventHandler<BasicDeliverEventArgs> action, CreateOrderOrchestration orchestrator)
        {
            consumer.Received += action;
            orchestrator.AcknowladgeReceivedMessage += onAcknowladgeReceivedMessage;
        }

        public void UnsubscribeFromChannel(EventHandler<BasicDeliverEventArgs> action)
        {
            consumer.Received -= action;
        }

        private void onAcknowladgeReceivedMessage(object sender, OrchestrationEventArgs args)
        {
            logger.LogInformation($"Exchange: {exchangeName} received rbmq message. Routing key: {queueName}");
            channel.BasicAck(args.DeliveryTag, false);
        }
    }
}