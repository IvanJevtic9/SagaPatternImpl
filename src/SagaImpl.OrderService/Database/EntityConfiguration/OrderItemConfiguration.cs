using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SagaImpl.OrderService.Entities;

namespace SagaImpl.OrderService.Database.EntityConfiguration
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItemEntity>
    {
        public void Configure(EntityTypeBuilder<OrderItemEntity> builder)
        {
            builder.ToTable("OrderItems", Consents.SchemaNames.ORDER_SCHEMA);

            builder.HasKey(o => o.Id);

            builder.Property(o => o.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(o => o.NumberOf)
                   .IsRequired();

            builder.Property(o => o.Price)
                   .IsRequired(); 

            builder.HasOne(o => o.Order)
                   .WithMany(o => o.OrderItems)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasForeignKey(o => o.OrderId)
                   .IsRequired();
        }
    }
}
