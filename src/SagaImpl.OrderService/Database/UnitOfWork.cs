using SagaImpl.OrderService.Database.Repository;
using SagaImpl.OrderService.Database.Repository.Interface;
using System.Threading.Tasks;
using SagaImpl.Common.Abstraction.Interface;
using System;

namespace SagaImpl.OrderService.Database
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OrderDbContext db;

        public IOrderStausRepository OrderStatus { get; set; }

        public IOrderRepository Order { get; set; }

        public IOrderItemRepository OrderItem { get; set; }

        public ISagaSessionRepository SagaSession { get; set; }

        public ILogTypeReposiotry LogType { get; set; }

        public ISagaLogRepository SagaLog { get; set; }

        public UnitOfWork(OrderDbContext db)
        {
            this.db = db;

            OrderStatus = new OrderStatusRepository(db);
            Order = new OrderRepository(db);
            OrderItem = new OrderItemRepository(db);
            SagaSession = new SagaSessionRepository(db);
            LogType = new LogTypeRepository(db);
            SagaLog = new SagaLogRepository(db);
        }

        public void Dispose()
        {
            db.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task SaveChangesAsync()
        {
            await db.SaveChangesAsync();
        }
    }
}
