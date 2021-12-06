using SagaImpl.Common.Apstraction.Interface;
using System;

namespace SagaImpl.Common.Saga
{
    public class SagaLog : IEntity
    {
        public int Id { get; set; }

        public int SessionId { get; set; }

        public int SagaDefinitionId { get; set; }

        public LogType LogType { get; set; }

        public string Log { get; set; }

        public DateTimeOffset LogTime { get; } = DateTimeOffset.UtcNow;
    }
}
