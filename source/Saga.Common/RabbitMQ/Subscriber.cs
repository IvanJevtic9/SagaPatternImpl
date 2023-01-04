using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Saga.Common.RabbitMQ.Abstraction;
using System.Text;

namespace Saga.Common.RabbitMQ
{
    public abstract class Subscriber : ISubscriber
    {
        protected bool disposed;

        protected readonly IModel channel;
        protected readonly string exchangeName;
        protected readonly string queueName;

        public Subscriber(
            IConnectionProvider connectionProvider,
            string queueName,
            string exchangeName,
            string exchangeType,
            int timeToLive = 30000)
        {
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
                var message = Encoding.UTF8.GetString(body);

                bool success = await callback.Invoke(message, e.BasicProperties.Headers);

                Console.WriteLine($"Exchange: {exchangeName} received rbmq message. Routing key: {queueName}");

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
