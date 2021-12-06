using SagaImpl.Common.Apstraction.Interface;

namespace SagaImpl.Common.Saga
{
    public class SagaSteps : IEntity
    {
        public int Id { get; set; }

        public int SagaDefinitionId { get; set; }

        public int Phase { get; set; }

        public string TransactionMethod { get; set; }

        public string CompensationMethod { get; set; }
    }
}
