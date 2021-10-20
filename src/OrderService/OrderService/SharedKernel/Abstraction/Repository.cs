using System.Collections.Generic;
using System.Linq;

namespace OrderService.SharedKernel.Abstraction
{
    public abstract class Repository
    {
        public void Add<T>(T entity)
        {
            GetTable(entity).Add(entity);
        }

        public void AddRange<T>(IEnumerable<T> collection) where T : new()
        {
            GetTable(new T()).AddRange(collection);
        }

        public void Remove<T>(T entity)
        {
            GetTable(entity).Remove(entity);
        }

        public void RemoveRange<T>(IEnumerable<T> collection) where T : new()
        {
            var t = GetTable(new T());

            foreach (var item in collection)
            {
                t.Remove(item);
            }
        }

        public void UpdateItem<T>(int id, T entity) where T : IEntity
        {
            var t = GetTable(entity);

            var item = t.FirstOrDefault(e => e.Id == id);

            item.DeepCopy(entity);
        }

        public abstract List<T> GetTable<T>(T entity);
    }
}
