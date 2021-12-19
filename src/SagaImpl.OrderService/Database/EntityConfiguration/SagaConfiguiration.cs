using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SagaImpl.Common.Saga;

namespace SagaImpl.OrderService.Database.EntityConfiguration
{
    public class LogTypeConfiguration : IEntityTypeConfiguration<LogType>
    {
        public void Configure(EntityTypeBuilder<LogType> builder)
        {
            builder.ToTable("LogTypes", Consents.SchemaNames.SAGA_SCHEMA);

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                   .ValueGeneratedNever();

            builder.Property(a => a.Name)
                   .HasMaxLength(20)
                   .IsRequired();
        }
    }

    public class SagaSessionConfiguration : IEntityTypeConfiguration<SagaSession>
    {
        public void Configure(EntityTypeBuilder<SagaSession> builder)
        {
            builder.ToTable("Sessions", Consents.SchemaNames.SAGA_SCHEMA);

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(a => a.Status)
                   .HasMaxLength(30);

            builder.Property(a => a.TimeCreated)
                   .IsRequired();
        }
    }

    public class SagaLogConfiguration : IEntityTypeConfiguration<SagaLog>
    {
        public void Configure(EntityTypeBuilder<SagaLog> builder)
        {
            builder.ToTable("Logs", Consents.SchemaNames.SAGA_SCHEMA);

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(a => a.Name)
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(a => a.LogTime)
                   .IsRequired();

            builder.HasOne(a => a.LogType)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasForeignKey(a => a.LogTypeId)
                   .IsRequired();

            builder.HasOne(a => a.Session)
                   .WithMany(a => a.Logs)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasForeignKey(a => a.SessionId)
                   .IsRequired();
        }
    }
}
