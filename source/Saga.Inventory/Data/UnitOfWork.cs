using Saga.Common.Data.Abstraction;
using Saga.Inventory.Data.Repository;
using Saga.Inventory.Data.Repository.Interface;

namespace Saga.Inventory.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly InventoryDbContext db;

        public IProductRepository Product { get; set; }

        public UnitOfWork(InventoryDbContext db)
        {
            this.db = db;

            Product = new ProductRepository(db);
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
