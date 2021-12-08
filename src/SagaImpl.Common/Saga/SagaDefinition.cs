using SagaImpl.Common.Apstraction.Interface;
using SagaImpl.Common.Saga.Abstraction;
using System.Collections.Generic;

namespace SagaImpl.Common.Saga
{
    public class SagaDefinition : ISaga , IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int NumberOfPhases { get; set; }

        public virtual List<SagaStep> Steps { get; set; }
    }
}
