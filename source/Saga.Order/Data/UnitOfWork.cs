using Saga.Common.Data.Abstraction;
using Saga.Order.Data.Repository;
using Saga.Order.Data.Repository.Interface;

namespace Saga.Order.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OrderDbContext db;

        public IPaymentOrderRepository PaymentOrder { get; set; }
        public IPaymentOrderItemRepository PaymentOrderItem { get; set; }
        public IProductRepository ProductRepository { get; set; }
        public ISagaSessionRepository SagaSession { get; set; }
        public ISagaLogRepository SagaLog { get; set; }

        public UnitOfWork(OrderDbContext db)
        {
            this.db = db;

            PaymentOrder = new PaymentOrderRepository(db);
            PaymentOrderItem = new PaymentOrderItemRepository(db);
            ProductRepository = new ProductRepository(db);
            SagaSession = new SagaSessionRepository(db);
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
