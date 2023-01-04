using Saga.Common.Data.Abstraction;

namespace Saga.Common.Entities.Saga
{
    public class SagaSession : IEntity
    {
        public int Id { get; set; }
        public string CorrelationId { get; set; }
        public int SagaLevel { get; set; }
        public string Status { get; set; }
        public string Context { get; set; }
        public string SagaName { get; set; }
        public bool RollbackTrigger { get; set; } = false;
        public DateTimeOffset TimeCreated { get; } = DateTimeOffset.UtcNow;
        public virtual ICollection<SagaLog> Logs { get; set; }
    }
}
