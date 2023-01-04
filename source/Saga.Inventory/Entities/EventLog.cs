using Saga.Common.Data.Abstraction;

namespace Saga.Inventory.Entities
{
    public class EventLog : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Log { get; set; }

        public DateTimeOffset DateTime { get; set; }
    }
}
