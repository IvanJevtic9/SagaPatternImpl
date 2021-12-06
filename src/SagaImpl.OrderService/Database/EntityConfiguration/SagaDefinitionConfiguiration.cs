using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SagaImpl.Common.Saga;

namespace SagaImpl.OrderService.Database.EntityConfiguration
{
    public class SagaDefinitionConfiguiration : IEntityTypeConfiguration<SagaDefinition>
    {
        public void Configure(EntityTypeBuilder<SagaDefinition> builder)
        {
            throw new System.NotImplementedException();
        }
    }
}
