using SagaImpl.Common.Messaging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SagaImpl.Common.Saga.Abstaction
{
    public interface IOrchestration
    {
        public bool IsAlive { get; }

        public Task StartAsync();

        public Task ContinueAsync();

        public void OnMessageReceive(string message, IDictionary<string, object> messageAttributes);
    }
}