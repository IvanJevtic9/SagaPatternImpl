using SagaImpl.Common.Messaging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SagaImpl.Common.Saga.Abstaction
{
    public interface IOrchestration
    {
        bool IsAlive { get; }

        Task StartAsync(object input);

        Task<bool> OnMessageReceive(string message, IDictionary<string, object> messageAttributes);
    }
}