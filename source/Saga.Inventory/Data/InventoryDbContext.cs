using Microsoft.EntityFrameworkCore;
using Saga.Inventory.Entities;

namespace Saga.Inventory.Data
{
    public class InventoryDbContext : DbContext
    {
        public InventoryDbContext(DbContextOptions options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(builder =>
            {
                builder.ToTable("Products", "Inventory");

                builder.HasKey(p => p.Id);
                builder.Property(p => p.Id)
                    .ValueGeneratedOnAdd();

                builder.Property(p => p.Name)
                    .HasMaxLength(254)
                    .IsRequired();

                builder.Property(p => p.Description)
                    .HasMaxLength(254)
                    .IsRequired();

                builder.Property(p => p.Quantity)
                    .IsRequired();

                builder.Property(p => p.Price)
                    .IsRequired();
            });

            modelBuilder.Entity<EventLog>(builder =>
            {
                builder.ToTable("EventLogs", "Inventory");

                builder.HasKey(p => p.Id);
                builder.Property(p => p.Id)
                    .ValueGeneratedOnAdd();

                builder.Property(p => p.Name)
                    .HasMaxLength(254)
                    .IsRequired();

                builder.Property(p => p.Log)
                    .IsRequired();
            });
        }
    }
}
