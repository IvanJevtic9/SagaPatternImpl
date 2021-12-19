using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SagaImpl.Common.Abstraction.Interface;
using SagaImpl.Common.Extension;
using SagaImpl.Common.RabbitMQ.Abstraction;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SagaImpl.Common.RabbitMQ
{
    public abstract class Subscriber<T> : ISubscriber where T : class
    {
        protected bool disposed;

        protected readonly IModel channel;
        protected readonly ILoggerAdapter<T> logger;

        protected readonly string exchangeName;
        protected readonly string queueName;

        public Subscriber(
            IConnectionProvider connectionProvider,
            ILoggerAdapter<T> logger,
            string queueName,
            string exchangeName,
            string exchangeType,
            int timeToLive = 30000)
        {
            this.logger = logger;
            this.exchangeName = exchangeName;
            this.queueName = queueName;

            var ttl = new Dictionary<string, object>
            {
                {"x-message-ttl", timeToLive }
            };

            channel = connectionProvider.GetConnection().CreateModel();
            channel.ExchangeDeclare(exchangeName, exchangeType, arguments: ttl);
            channel.QueueDeclare(queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            channel.QueueBind(queueName, exchangeName, queueName);
        }

        public virtual void SubscribeAsync(Func<string, IDictionary<string, object>, Task<bool>> callback)
        {
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (sender, e) =>
            {
                var body = e.Body.ToArray();
                var message = body.GetString();

                bool success = await callback.Invoke(message, e.BasicProperties.Headers);

                logger.LogInformation($"Exchange: {exchangeName} received rbmq message. Routing key: {queueName}");

                if (success) channel.BasicAck(e.DeliveryTag, false);
            };

            channel.BasicConsume(queueName, false, consumer);
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
