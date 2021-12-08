using SagaImpl.Common.Apstraction.Interface;
using System;
using System.Collections.Generic;

namespace SagaImpl.Common.Saga
{
    public class SagaSession : IEntity
    {
        public int Id { get; set; }

        public int SagaDefinitionId { get; set; }

        public virtual SagaDefinition SagaDefinition { get; set; }

        public string Status { get; set; }

        public DateTimeOffset TimeCreated { get; } = DateTimeOffset.UtcNow;

        public virtual List<SagaLog> Logs { get; set; }
    }
}
