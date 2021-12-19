using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SagaImpl.OrderService.Entities;

namespace SagaImpl.OrderService.Database.EntityConfiguration
{
    public class OrderConfiguration : IEntityTypeConfiguration<OrderEntity>
    {
        public void Configure(EntityTypeBuilder<OrderEntity> builder)
        {
            builder.ToTable("Orders", Consents.SchemaNames.ORDER_SCHEMA);

            builder.HasKey(o => o.Id);

            builder.Property(o => o.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(o => o.UserId)
                   .IsRequired();

            builder.Property(o => o.CreatedDate)
                   .IsRequired();

            builder.Property(o => o.TotalPrice)
                   .IsRequired();

            builder.HasOne(a => a.Status)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasForeignKey(a => a.StatusId)
                   .IsRequired();
        }
    }

    public class OrderStatusConfiguration : IEntityTypeConfiguration<OrderStatus>
    {
        public void Configure(EntityTypeBuilder<OrderStatus> builder)
        {
            builder.ToTable("OrderStatuses", Consents.SchemaNames.ORDER_SCHEMA);

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                   .ValueGeneratedNever();

            builder.Property(a => a.Name)
                   .HasMaxLength(20)
                   .IsRequired();
        }
    }
}
