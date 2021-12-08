using Microsoft.EntityFrameworkCore.Storage;
using SagaImpl.OrderService.Database.Repository;
using SagaImpl.OrderService.Database.Repository.Interface;
using System.Threading.Tasks;
using SagaImpl.Common.Abstraction.Interface;

namespace SagaImpl.OrderService.Database
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OrderDbContext db;

        public IOrderRepository Order { get; set; }

        public IOrderItemRepository OrderItem { get; set; }

        public ISagaDefinitionRepository SagaDefinition { get; set; }

        public ISagaStepRepository SagaStep { get; set; }

        public ISagaSessionRepository SagaSession { get; set; }

        public ISagaLogRepository SagaLog { get; set; }

        public UnitOfWork(OrderDbContext db)
        {
            this.db = db;

            Order = new OrderRepository(db);
            OrderItem = new OrderItemRepository(db);
            SagaDefinition = new SagaDefinitionRepository(db);
            SagaStep = new SagaStepRepository(db);
            SagaSession = new SagaSessionRepository(db);
            SagaLog = new SagaLogRepository(db);
        }

        public IDbContextTransaction BeginTransaction()
        {
            return db.Database.BeginTransaction();
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public async Task SaveChangesAsync()
        {
            await db.SaveChangesAsync();
        }
    }
}
