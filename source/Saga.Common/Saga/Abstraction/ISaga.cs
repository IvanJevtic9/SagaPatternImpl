namespace Saga.Common.Saga.Abstraction
{
    public interface ISaga
    {
        public string Name { get; }
        public List<(int Level, ISagaStep Step)> Steps { get; }
    }
}
