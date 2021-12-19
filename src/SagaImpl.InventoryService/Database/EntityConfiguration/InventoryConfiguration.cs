using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SagaImpl.InventoryService.Entities;

namespace SagaImpl.InventoryService.Database.EntityConfiguration
{
    public class ProductConfiguration : IEntityTypeConfiguration<ProductEntity>
    {
        public void Configure(EntityTypeBuilder<ProductEntity> builder)
        {
            builder.ToTable("Products", Consents.SchemaNames.PRODUCT_SCHEMA);

            builder.HasKey(o => o.Id);

            builder.Property(o => o.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(o => o.Name)
                   .HasMaxLength(255)
                   .IsRequired();

            builder.Property(o => o.Description)
                   .HasMaxLength(1000);

            builder.Property(o => o.Quantity)
                   .IsRequired();

            builder.Property(o => o.Price)
                   .IsRequired();
        }
    }
}
