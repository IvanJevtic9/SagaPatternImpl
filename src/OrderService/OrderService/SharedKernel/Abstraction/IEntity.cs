namespace OrderService.SharedKernel.Abstraction
{
    public interface IEntity
    {
        public int Id { get; }

        public void DeepCopy(IEntity entity);
    }
}
