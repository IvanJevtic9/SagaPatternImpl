using SagaImpl.Common.Apstraction.Implementation;
using SagaImpl.Common.Saga;
using SagaImpl.OrderService.Database.Repository.Interface;

namespace SagaImpl.OrderService.Database.Repository
{
    public class SagaSessionRepository : Repository<SagaSession>, ISagaSessionRepository
    {
        public SagaSessionRepository(OrderDbContext db) : base(db)
        { }
    }

    public class LogTypeRepository : Repository<LogType>, ILogTypeReposiotry
    {
        public LogTypeRepository(OrderDbContext db) : base(db)
        { }
    }

    public class SagaLogRepository : Repository<SagaLog>, ISagaLogRepository
    {
        public SagaLogRepository(OrderDbContext db) : base(db)
        { }
    }
}
