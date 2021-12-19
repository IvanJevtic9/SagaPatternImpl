using SagaImpl.Common.Apstraction.Interface;
using System;

namespace SagaImpl.Common.Saga
{
    public class SagaLog : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Log { get; set; }

        public DateTimeOffset LogTime { get; } = DateTimeOffset.UtcNow;

        public int LogTypeId { get; set; }

        public virtual LogType LogType { get; set; }

        public int SessionId { get; set; }

        public virtual SagaSession Session { get; set; }
    }
}
