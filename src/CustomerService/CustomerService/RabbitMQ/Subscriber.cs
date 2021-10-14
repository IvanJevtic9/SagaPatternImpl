using MessageBroker.RabbitMQ;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerService.RabbitMQ
{
    public class Subscriber : ISubscriber
    {
        private readonly string queue;
        private readonly IModel model;
        private bool disposed;

        public Subscriber(IConnectionProvider connectionProvider, string exchange, string queue, string routingKey, string exchangeType)
        {
            this.queue = queue;
            this.model = connectionProvider.GetConnection().CreateModel();

            model.ExchangeDeclare(exchange, exchangeType);
            model.QueueDeclare(
                    queue,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

            model.QueueBind(queue, exchange, routingKey);
        }

        public void Subscribe(Func<string, IDictionary<string, object>, bool> callback)
        {
            var consumer = new EventingBasicConsumer(model);

            consumer.Received += (sender, args) =>
            {
                var body = args.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                bool success = callback.Invoke(message, args.BasicProperties.Headers);
                if(success)
                {
                    model.BasicAck(args.DeliveryTag, true);
                }
            };

            model.BasicConsume(queue, false, consumer);
        }

        public void SubscribeAsync(Func<string, IDictionary<string, object>, Task<bool>> callback)
        {
            var consumer = new AsyncEventingBasicConsumer(model);

            consumer.Received += async (sender, args) =>
            {
                var body = args.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                bool success = await callback.Invoke(message, args.BasicProperties.Headers);
                if (success)
                {
                    model.BasicAck(args.DeliveryTag, true);
                }
            };

            model.BasicConsume(queue, false, consumer);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
                model?.Close();

            disposed = true;
        }
    }
}
