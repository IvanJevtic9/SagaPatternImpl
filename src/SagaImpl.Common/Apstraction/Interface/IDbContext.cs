using Microsoft.EntityFrameworkCore;

namespace SagaImpl.Common.Apstraction.Interface
{
    public interface IDbContext
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        void Dispose();

        string GetConnectionString();
    }
}
