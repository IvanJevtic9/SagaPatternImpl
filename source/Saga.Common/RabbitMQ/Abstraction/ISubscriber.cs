namespace Saga.Common.RabbitMQ.Abstraction
{
    public interface ISubscriber : IDisposable
    {
        void SubscribeAsync(Func<string, IDictionary<string, object>, Task<bool>> callback);
    }
}