using Microsoft.EntityFrameworkCore;
using Saga.Common.Data;
using Saga.Common.Entities.Saga;
using Saga.Order.Data.Repository.Interface;

namespace Saga.Order.Data.Repository
{
    public class SagaLogRepository : Repository<SagaLog>, ISagaLogRepository
    {
        public SagaLogRepository(DbContext db) : base(db)
        { }
    }
}
