using Saga.Common.Messaging.Abstraction;
using Saga.Common.Saga.Abstraction;

namespace Saga.Common.Messaging
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _serviceScope;

        public EventProcessor(IServiceScopeFactory serviceScope)
        {
            _serviceScope = serviceScope;
        }

        public async Task ProcessEvent<TEvent>(Event<TEvent> message) where TEvent : class
        {
            var scope = _serviceScope.CreateScope();

            var orchestration = scope.ServiceProvider.GetRequiredService<IOrchestration>();
            var saga = scope.ServiceProvider.GetRequiredService<ISaga>();

            await orchestration.LoadFromCorrelationId(message.CorrelationId);
            orchestration.SetSaga(saga);

            if(!string.IsNullOrEmpty(message.ModelKey))
            {
                await orchestration.AddContextData(message.ModelKey, message.Model);
            }

            if(message.Type == EventType.Failure)
            {
                await orchestration.Rollback();
                return;
            }

            await orchestration.Continue();
        }
    }
}
