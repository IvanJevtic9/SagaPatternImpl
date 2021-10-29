using RabbitMQ.Client;
using SagaImpl.Common.Abstraction.Interface;
using SagaImpl.Common.RabbitMQ.Abstraction;
using System;
using System.Text;
using System.Collections.Generic;

namespace SagaImpl.Common.RabbitMQ.Sender
{
    public class OrderPublisher : IPublisher
    {
        private bool disposed;
        private readonly IModel channel;
        private readonly ILoggerAdapter<OrderPublisher> logger;

        public OrderPublisher(IConnectionProvider connectionProvider, ILoggerAdapter<OrderPublisher> logger, string exchangeType, int timeToLive = 30000)
        {
            this.logger = logger;
            channel = connectionProvider.GetConnection().CreateModel();
            
            var ttl = new Dictionary<string, object>
            {
                {"x-message-ttl", timeToLive }
            };

            channel.ExchangeDeclare(CommonConstants.ORDER_SERVICE_EXCHANGE, exchangeType, arguments: ttl);
        }

        public void Publish(string message, string queueName, IDictionary<string, object> messageAttributes, string timeToLive = "30000")
        {
            var body = Encoding.UTF8.GetBytes(message);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.Headers = messageAttributes;
            properties.Expiration = timeToLive;

            channel.BasicPublish(CommonConstants.ORDER_SERVICE_EXCHANGE, queueName, properties, body);

            logger.LogInformation($"Exchange: {CommonConstants.ORDER_SERVICE_EXCHANGE} sent rbmq message. Routing key: {queueName}");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;
            if (disposing) channel?.Close();
            disposed = true;
        }
    }
}