using RabbitMQ.Client;
using System;

namespace SagaImpl.Common.RabbitMQ.Abstraction
{
    public interface IConnectionProvider : IDisposable
    {
        IConnection GetConnection();
    }
}