using RabbitMQ.Client;
using Saga.Common.RabbitMQ.Abstraction;
using Saga.Common.Settings;

namespace Saga.Common.RabbitMQ
{
    public class ConnectionProvider : IConnectionProvider
    {
        private readonly ConnectionFactory factory;
        private readonly IConnection connection;
        private bool disposed;

        public ConnectionProvider(RabbitMQSettings settings)
        {
            factory = new ConnectionFactory()
            {
                Uri = new Uri(settings.Uri)
            };

            try
            {
                connection = factory.CreateConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not create a connection {0}", ex.Message);
            }
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
            if (disposed) return;
            if (disposing) connection?.Close();
            disposed = true;
        }
    }
}
