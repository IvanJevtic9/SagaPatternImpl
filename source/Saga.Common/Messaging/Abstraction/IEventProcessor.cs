namespace Saga.Common.Messaging.Abstraction
{
    public interface IEventProcessor
    {
        Task ProcessEvent<TEvent>(Event<TEvent> message) where TEvent : class;
    }
}
