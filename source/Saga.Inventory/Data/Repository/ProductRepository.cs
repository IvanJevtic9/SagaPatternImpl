using Microsoft.EntityFrameworkCore;
using Saga.Common.Data;
using Saga.Inventory.Data.Repository.Interface;
using Saga.Inventory.Entities;

namespace Saga.Inventory.Data.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(DbContext db) : base(db)
        { }
    }
}
