namespace Saga.Common.Messaging.Abstraction
{
    public class Event<IMessage> where IMessage : class
    {
        public string CorrelationId { get; set; }
        public string ModelKey { get; set; }
        public IMessage Model { get; set; }
        public EventType Type { get; set; }
        public DateTime CreationDate { get; } = DateTime.UtcNow;
    }
}
