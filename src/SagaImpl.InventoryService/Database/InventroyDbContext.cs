using Microsoft.EntityFrameworkCore;
using SagaImpl.Common.Apstraction.Interface;
using SagaImpl.InventoryService.Database.EntityConfiguration;

namespace SagaImpl.InventoryService.Database
{
    public class InventoryDbContext : DbContext, IDbContext
    {
        public InventoryDbContext(DbContextOptions options) : base(options)
        { }

        public string GetConnectionString()
        {
            return Database.GetDbConnection().ConnectionString;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
        }
    }
}
