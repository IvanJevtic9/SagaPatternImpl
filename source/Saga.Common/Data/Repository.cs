using Microsoft.EntityFrameworkCore;
using Saga.Common.Data.Abstraction;
using System.Linq.Expressions;

namespace Saga.Common.Data
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class , IEntity
    {
        internal DbSet<TEntity> dbSet;

        public Repository(DbContext db)
        {
            dbSet = db.Set<TEntity>();
        }

        public async Task AddAsync(TEntity entity)
        {
            await dbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await dbSet.AddRangeAsync(entities);
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await dbSet.AnyAsync(predicate);
        }

        public void Attach(TEntity entity)
        {
            dbSet.Attach(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>,
            IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (predicate != null) query = await Task.Run(() => { return dbSet.Where(predicate); });
            if (includeProperties != null)
            {
                foreach (var property in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                    query = query.Include(property);
            }

            if (orderBy != null) return orderBy(query).ToList();

            return query.ToList();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate = null, string includeProperties = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (predicate != null) query = query.Where(predicate);
            if (includeProperties != null)
            {
                foreach (var property in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                    query = query.Include(property);
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task RemoveAsync(int id)
        {
            var entity = await dbSet.FindAsync(id);

            dbSet.Remove(entity);
        }

        public async Task RemoveAsync(TEntity entity)
        {
            await Task.Run(() => dbSet.Remove(entity));
        }

        public async Task RemoveRangeAsync(IEnumerable<TEntity> entities)
        {
            await Task.Run(() => dbSet.RemoveRange(entities));
        }
    }
}
