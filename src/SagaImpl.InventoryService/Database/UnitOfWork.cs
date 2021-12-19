using SagaImpl.Common.Abstraction.Interface;
using SagaImpl.InventoryService.Database.Repository;
using SagaImpl.InventoryService.Database.Repository.Interface;
using System;
using System.Threading.Tasks;

namespace SagaImpl.InventoryService.Database
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
