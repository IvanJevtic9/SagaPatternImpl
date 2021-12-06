using ELKOMERC.SharedKernel.Abstraction.Interface;
using Microsoft.EntityFrameworkCore.Storage;
using SagaImpl.OrderService.Database.Repository;
using SagaImpl.OrderService.Database.Repository.Interface;
using System.Threading.Tasks;

namespace SagaImpl.OrderService.Database
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OrderDbContext db;

        public IOrderRepository Order { get; set; }

        public IOrderItemRepository OrderItem { get; set; }

        public UnitOfWork(OrderDbContext db)
        {
            this.db = db;

            Order = new OrderRepository(db);
            OrderItem = new OrderItemRepository(db);
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
