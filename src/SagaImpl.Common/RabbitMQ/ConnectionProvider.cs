using RabbitMQ.Client;
using SagaImpl.Common.Abstraction.Interface;
using SagaImpl.Common.RabbitMQ.Abstraction;
using SagaImpl.Common.Settings;
using System;

namespace SagaImpl.Common.RabbitMQ
{
    public class ConnectionProvider : IConnectionProvider
    {
        private readonly ConnectionFactory factory;
        private readonly IConnection connection;
        private bool disposed;

        public ConnectionProvider(RabbitMQSettings settings, ILoggerAdapter<ConnectionProvider> logger)
        {
            factory = new ConnectionFactory()
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672")
                //HostName = settings.Host,
                //Port = settings.Port,
                //UserName = settings.UserName,
                //Password = settings.Password
            };

            try
            {
                connection = factory.CreateConnection();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Could not create a connection {0}", ex.Message);
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
            if (disposed)
                return;

            if (disposing)
                connection?.Close();

            disposed = true;
        }
    }
}
