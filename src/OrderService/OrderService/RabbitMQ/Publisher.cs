using MessageBroker.RabbitMQ;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.RabbitMQ
{
    public class Publisher : IPublisher
    {
        private bool disposed;
        private readonly IModel model;
        private readonly string exchange;

        public Publisher(IConnectionProvider connectionProvider, string exchange, string exchangeType)
        {
            this.exchange = exchange;
            model = connectionProvider.GetConnection().CreateModel();
            model.ExchangeDeclare(exchange, exchangeType);
        }

        public void Publish(string message, string routeKey, IDictionary<string, object> messageAttributes)
        {
            var body = Encoding.UTF8.GetBytes(message);

            var properties = model.CreateBasicProperties();
            properties.Headers = messageAttributes;
            properties.Persistent = true;

            model.BasicPublish(exchange, routeKey, properties, body);
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                model.Dispose();
            }

            disposed = true;
        }
    }
}
