using OrderService.SharedKernel.Abstraction;

namespace OrderService.Entity
{
    public class SagaDefinitionEntity : IEntity
    {
        public int Id { get; init; }

        public void DeepCopy(IEntity entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
