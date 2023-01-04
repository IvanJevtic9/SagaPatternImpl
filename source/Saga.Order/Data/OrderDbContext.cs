using Microsoft.EntityFrameworkCore;
using Saga.Common.Entities.Saga;
using Saga.Order.Entities;

namespace Saga.Order.Data
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PaymentOrder>(builder =>
            {
                builder.ToTable("Orders", "Order");

                builder.HasKey(p => p.Id);
                builder.Property(o => o.Id)
                   .ValueGeneratedOnAdd();

                builder.Property(o => o.UserId)
                   .IsRequired();

                builder.Property(o => o.CreateDate)
                       .IsRequired();

                builder.Property(o => o.TotalPrice)
                       .IsRequired();
            });

            modelBuilder.Entity<PaymentOrderItem>(builder =>
            {
                builder.ToTable("OrderItems", "Order");

                builder.HasKey(a => a.Id);

                builder.Property(a => a.Id)
                       .ValueGeneratedOnAdd();

                builder.Property(o => o.NumberOf)
                   .IsRequired();

                builder.Property(o => o.Price)
                       .IsRequired();

                builder.HasOne(o => o.Order)
                       .WithMany(o => o.Items)
                       .OnDelete(DeleteBehavior.Cascade)
                       .HasForeignKey(o => o.OrderId)
                       .IsRequired();
            });

            modelBuilder.Entity<Product>(builder =>
            {
                builder.ToTable("Products", "Order");

                builder.HasKey(a => a.Id);

                builder.Property(a => a.Id)
                       .ValueGeneratedOnAdd();

                builder.Property(o => o.ProductId)
                   .IsRequired();

                builder.Property(o => o.Price)
                   .IsRequired();
            });

            modelBuilder.Entity<SagaSession>(builder =>
            {
                builder.ToTable("SagaSessions", "Order");

                builder.HasKey(a => a.Id);

                builder.Property(a => a.Id)
                       .ValueGeneratedOnAdd();

                builder.Property(o => o.CorrelationId)
                   .IsRequired();

                builder.Property(o => o.Status)
                   .IsRequired();

                builder.Property(o => o.TimeCreated)
                   .IsRequired();

                builder.Property(o => o.Status)
                   .IsRequired();
            });

            modelBuilder.Entity<SagaLog>(builder =>
            {
                builder.ToTable("SagaLogs", "Order");

                builder.HasKey(a => a.Id);

                builder.Property(a => a.Id)
                       .ValueGeneratedOnAdd();

                builder.Property(o => o.Step)
                   .IsRequired();

                builder.Property(o => o.Log)
                   .IsRequired();

                builder.Property(o => o.TypeId)
                   .IsRequired();

                builder.Property(o => o.Type)
                   .IsRequired();

                builder.HasOne(o => o.Session)
                       .WithMany(o => o.Logs)
                       .OnDelete(DeleteBehavior.Cascade)
                       .HasForeignKey(o => o.SessionId)
                       .IsRequired();
            });
        }
    }
}
