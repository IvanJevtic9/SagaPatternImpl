using RabbitMQ.Client;

namespace MessageBroker.RabbitMQ
{
    public interface IConnectionProvider
    {
        IConnection GetConnection();
        void Dispose(); 
    }
}