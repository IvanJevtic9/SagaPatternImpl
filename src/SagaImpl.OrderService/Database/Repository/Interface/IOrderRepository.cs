using SagaImpl.Common.Abstraction.Interface;
using SagaImpl.OrderService.Entities;

namespace SagaImpl.OrderService.Database.Repository.Interface
{
    public interface IOrderStausRepository : IRepository<OrderStatus>
    { }

    public interface IOrderRepository : IRepository<OrderEntity>
    { }

    public interface IOrderItemRepository : IRepository<OrderItemEntity>
    { }
}