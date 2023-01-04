namespace Saga.Common.Saga.Abstraction
{
    public interface ISagaStep
    {
        public string CorrelationId { get; }
        public string StepName { get; }
        Task SetContext(int sessionId, string correlationId, IDictionary<string, object> context);
        Task Commit();
        Task RollBack();
        Task Complete();
    }
}
