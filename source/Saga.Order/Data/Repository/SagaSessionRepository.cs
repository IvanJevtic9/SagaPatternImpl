using Microsoft.EntityFrameworkCore;
using Saga.Common.Data;
using Saga.Common.Entities.Saga;
using Saga.Order.Data.Repository.Interface;

namespace Saga.Order.Data.Repository
{
    public class SagaSessionRepository : Repository<SagaSession>, ISagaSessionRepository
    {
        public SagaSessionRepository(DbContext db) : base(db)
        { }
    }
}
