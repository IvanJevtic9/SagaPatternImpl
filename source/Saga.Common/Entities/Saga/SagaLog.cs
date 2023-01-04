using Saga.Common.Data.Abstraction;

namespace Saga.Common.Entities.Saga
{
    public class SagaLog : IEntity
    {
        public int Id { get; set; }
        public string Step { get; set; }
        public string Log { get; set; }
        public LogType TypeId { get; set; }
        public string Type { get; set; }
        public int SessionId { get; set; }
        public virtual SagaSession Session { get; set; }
    }
}
