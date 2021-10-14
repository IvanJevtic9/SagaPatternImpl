using System;
using System.Collections.Generic;

namespace CustomerService.RabbitMQ
{
    public interface IPublisher : IDisposable
    {
        void Publish(string message, string routeKey, IDictionary<string, object> messageAttributes);
    }
}
