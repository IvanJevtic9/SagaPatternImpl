using SagaImpl.Common.Apstraction.Implementation;
using SagaImpl.Common.Apstraction.Interface;
using SagaImpl.InventoryService.Database.Repository.Interface;
using SagaImpl.InventoryService.Entities;

namespace SagaImpl.InventoryService.Database.Repository
{
    public class ProductRepository : Repository<ProductEntity>, IProductRepository
    {
        public ProductRepository(IDbContext db) : base(db)
        { }
    }
}
