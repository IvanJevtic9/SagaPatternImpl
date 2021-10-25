using Microsoft.EntityFrameworkCore;
using SagaImpl.Common.Apstraction.Interface;

namespace SagaImpl.InventoryService
{
    internal class InventoryDbContext : DbContext, IDbContext
    {
        public InventoryDbContext(DbContextOptions options) : base(options)
        { }

        public string GetConnectionString()
        {
            return Database.GetDbConnection().ConnectionString;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new OrderConfiguration());
        }
    }
}