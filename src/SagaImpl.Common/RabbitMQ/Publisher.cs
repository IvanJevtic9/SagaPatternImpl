using RabbitMQ.Client;
using SagaImpl.Common.Abstraction.Interface;
using SagaImpl.Common.RabbitMQ.Abstraction;
using System;
using System.Collections.Generic;
using System.Text;

namespace SagaImpl.Common.RabbitMQ
{
    public abstract class Publisher<T> : IPublisher where T : class
    {
        protected bool disposed;

        protected readonly IModel channel;
        protected readonly ILoggerAdapter<T> logger;

        protected readonly string exchangeName;

        public Publisher(
            IConnectionProvider connectionProvider,
            ILoggerAdapter<T> logger,
            string exchangeName,
            string exchangeType,
            int timeToLive = 30000)
        {
            this.exchangeName = exchangeName;

            this.logger = logger;
            channel = connectionProvider.GetConnection().CreateModel();

            var ttl = new Dictionary<string, object>
            {
                {"x-message-ttl", timeToLive }
            };

            channel.ExchangeDeclare(exchangeName, exchangeType, arguments: ttl);
        }

        public virtual void Publish(string message, string queueName, IDictionary<string, object> messageAttributes, string timeToLive = null)
        {
            var body = Encoding.UTF8.GetBytes(message);

            var properties = channel.CreateBasicProperties();
            
            properties.Persistent = true;
            properties.Headers = messageAttributes;
            properties.Expiration = timeToLive;

            channel.BasicPublish(exchangeName, queueName, properties, body);

            logger.LogInformation($"Exchange: {exchangeName} sent rbmq message. Routing key: {queueName}");
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
