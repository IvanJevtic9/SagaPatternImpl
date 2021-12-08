using SagaImpl.Common.Abstraction.Interface;
using SagaImpl.Common.Saga;

namespace SagaImpl.OrderService.Database.Repository.Interface
{
    public interface ISagaDefinitionRepository : IRepository<SagaDefinition>
    { }

    public interface ISagaStepRepository : IRepository<SagaStep>
    { }

    public interface ISagaSessionRepository : IRepository<SagaSession>
    { }

    public interface ISagaLogRepository : IRepository<SagaLog>
    { }
}
