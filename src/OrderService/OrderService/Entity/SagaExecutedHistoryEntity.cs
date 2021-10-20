using OrderService.SharedKernel.Abstraction;

namespace OrderService.Entity
{
    public class SagaExecutedHistoryEntity : IEntity
    {
        public int Id { get; init; }

        public void DeepCopy(IEntity entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
