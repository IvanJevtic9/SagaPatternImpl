using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SagaImpl.Common.Saga;

namespace SagaImpl.OrderService.Database.EntityConfiguration
{
    public class SagaDefinitionConfiguiration : IEntityTypeConfiguration<SagaDefinition>
    {
        public void Configure(EntityTypeBuilder<SagaDefinition> builder)
        {
            builder.ToTable("Definitions", Consents.SchemaNames.SAGA_SCHEMA);

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(a => a.Name)
                   .IsRequired()
                   .HasMaxLength(254);

            builder.Property(a => a.NumberOfPhases)
                   .IsRequired();
        }
    }

    public class SagaStepsConfiguiration : IEntityTypeConfiguration<SagaStep>
    {
        public void Configure(EntityTypeBuilder<SagaStep> builder)
        {
            builder.ToTable("Steps", Consents.SchemaNames.SAGA_SCHEMA);

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(a => a.Phase)
                   .IsRequired();

            builder.Property(a => a.TransactionMethod)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(a => a.CompensationMethod)
                   .HasMaxLength(255);

            builder.HasOne(a => a.Definition)
                   .WithMany(a => a.Steps)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasForeignKey(a => a.DefinitionId)
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

            builder.HasOne(a => a.SagaDefinition)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasForeignKey(a => a.SagaDefinitionId)
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

            builder.Property(a => a.LogType)
                   .IsRequired();

            builder.Property(a => a.LogTime)
                   .IsRequired();

            builder.HasOne(a => a.Session)
                   .WithMany(a => a.Logs)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasForeignKey(a => a.SessionId)
                   .IsRequired();
        }
    }
}
