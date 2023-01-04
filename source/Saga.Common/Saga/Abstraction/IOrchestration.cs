using Saga.Common.Entities.Saga;

namespace Saga.Common.Saga.Abstraction
{
    public interface IOrchestration
    {
        public IDictionary<string, object> Context { get; }
        public ISaga Saga { get; }
        public SagaSession Session { get; }

        void SetSaga(ISaga saga);
        Task AddContextData(string key, object data);
        Task RemoveContextData(string key);
        Task<bool> LoadFromCorrelationId(string correlationId);
        Task Start();
        Task Continue();
        Task Rollback();
    }
}
