using SagaImpl.Common.Abstraction.Interface;
using SagaImpl.Common.Saga;

namespace SagaImpl.OrderService.Database.Repository.Interface
{
    public interface ISagaSessionRepository : IRepository<SagaSession>
    { }

    public interface ILogTypeReposiotry : IRepository<LogType>
    { }

    public interface ISagaLogRepository : IRepository<SagaLog>
    { }
}
