using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerService.RabbitMQ
{
    public interface ISubscriber
    {
        void Subscribe(Func<string, IDictionary<string, object>, bool> callback);
        void SubscribeAsync(Func<string, IDictionary<string, object>, Task<bool>> callback);
    }
}
