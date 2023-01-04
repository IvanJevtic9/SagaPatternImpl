using RabbitMQ.Client;

namespace Saga.Common.RabbitMQ.Abstraction
{
    public interface IConnectionProvider : IDisposable
    {
        IConnection GetConnection();
    }
}