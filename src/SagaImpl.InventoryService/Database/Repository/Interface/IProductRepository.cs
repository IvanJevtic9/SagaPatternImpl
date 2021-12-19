using SagaImpl.Common.Abstraction.Interface;
using SagaImpl.InventoryService.Entities;

namespace SagaImpl.InventoryService.Database.Repository.Interface
{
    public interface IProductRepository : IRepository<ProductEntity>
    { }
}
