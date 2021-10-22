using Microsoft.EntityFrameworkCore;
using SagaImpl.Common.Apstraction.Interface;
using SagaImpl.OrderService.Database.EntityConfiguration;
using System;

namespace SagaImpl.OrderService.Database
{
    public class OrderDbContext : DbContext, IDbContext
    {
        public OrderDbContext(DbContextOptions options) : base(options)
        {}

        public string GetConnectionString()
        {
            Console.WriteLine($"{Database.GetDbConnection().ConnectionString}");
            return Database.GetDbConnection().ConnectionString;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderItemConfiguration());
        }
    }
}
