using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SagaImpl.Common.RabbitMQ.Abstraction
{
    public interface ISubscriber : IDisposable
    {
        void SubscribeAsync(Func<string, IDictionary<string, object>, Task<bool>> callback);
    }
}