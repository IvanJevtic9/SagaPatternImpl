using OrderService.Entity;
using OrderService.SharedKernel.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderService.Database
{
    //database simulation - not important in this context
    public class OrderServiceDatabase : Repository
    {
        public OrderServiceDatabase()
        {}

        public List<OrderEntity> OrderTable { get; private set; }
        public List<SagaDefinitionEntity> SagaDefinitionTable { get; private set; }
        public List<SagaExecutedHistoryEntity> SagaExecutedHistoryTable { get; private set; }

        public override List<T> GetTable<T>(T entity)
        {
            if(typeof(OrderEntity) == entity.GetType())
            {
                return OrderTable as List<T>;
            }
            else if (typeof(SagaDefinitionEntity) == entity.GetType())
            {
                return SagaDefinitionTable as List<T>;
            }
            else
            {
                return SagaExecutedHistoryTable as List<T>;
            }
        }
    }
}
