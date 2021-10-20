using System;
using RabbitMQ.Client;

namespace MessageBroker.RabbitMQ
{
    public class ConnectionProvider : IConnectionProvider
    {
        private readonly ConnectionFactory factory;
        private readonly IConnection connection;
        private bool disposed;
        
        public ConnectionProvider(string url)
        {
            factory = new ConnectionFactory
            {
                Uri = new Uri(url)
            };
            connection = factory.CreateConnection();
        }
        
        public IConnection GetConnection()
        {
            return connection;
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
                connection.Dispose();
            }

            disposed = true;
        }
    }
}