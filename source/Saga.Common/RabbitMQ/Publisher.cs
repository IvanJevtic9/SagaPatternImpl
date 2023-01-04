using RabbitMQ.Client;
using Saga.Common.RabbitMQ.Abstraction;
using System.Text;

namespace Saga.Common.RabbitMQ
{
    public abstract class Publisher : IPublisher
    {
        protected bool disposed;

        protected readonly IModel channel;
        protected readonly string exchangeName;

        public Publisher(
            IConnectionProvider connectionProvider,
            string exchangeName,
            string exchangeType,
            int timeToLive = 30000)
        {
            this.exchangeName = exchangeName;
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

            Console.WriteLine($"Exchange: {exchangeName} sent rbmq message. Routing key: {queueName}");
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
