using System;
using System.Collections.Generic;

namespace SagaImpl.Common.RabbitMQ.Abstraction
{
    public interface IPublisher : IDisposable
    {
        void Publish(string message, string routingKey, IDictionary<string, object> messageAttributes, string timeToLive = null);
    }
}