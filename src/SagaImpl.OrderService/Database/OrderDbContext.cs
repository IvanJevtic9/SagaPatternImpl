using Microsoft.EntityFrameworkCore;
using SagaImpl.Common.Apstraction.Interface;
using SagaImpl.OrderService.Database.EntityConfiguration;

namespace SagaImpl.OrderService.Database
{
    public class OrderDbContext : DbContext, IDbContext
    {
        public OrderDbContext(DbContextOptions options) : base(options)
        {}

        public string GetConnectionString()
        {
            return Database.GetDbConnection().ConnectionString;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderItemConfiguration());
            modelBuilder.ApplyConfiguration(new SagaDefinitionConfiguiration());
            modelBuilder.ApplyConfiguration(new SagaStepsConfiguiration());
            modelBuilder.ApplyConfiguration(new SagaSessionConfiguration());
            modelBuilder.ApplyConfiguration(new SagaLogConfiguration());
        }
    }
}
