using Microsoft.EntityFrameworkCore;
using Saga.Common.Data;
using Saga.Order.Data.Repository.Interface;
using Saga.Order.Entities;

namespace Saga.Order.Data.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(DbContext db) : base(db)
        { }
    }
}
