﻿using SagaImpl.Common.Apstraction.Interface;

namespace SagaImpl.Common.Saga
{
    public class SagaStep : IEntity
    {
        public int Id { get; set; }

        public int DefinitionId { get; set; }

        public virtual SagaDefinition Definition { get; set; }

        public int Phase { get; set; }

        public string TransactionMethod { get; set; }

        public string CompensationMethod { get; set; }
    }
}