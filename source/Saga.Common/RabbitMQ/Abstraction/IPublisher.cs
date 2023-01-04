namespace Saga.Common.RabbitMQ.Abstraction
{
    public interface IPublisher : IDisposable
    {
        void Publish(string message, string queueName, IDictionary<string, object> messageAttributes, string timeToLive = null);
    }
}