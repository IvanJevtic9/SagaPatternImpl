using SagaImpl.Common.Apstraction.Implementation;
using SagaImpl.OrderService.Database.Repository.Interface;
using SagaImpl.OrderService.Entities;

namespace SagaImpl.OrderService.Database.Repository
{
    public class OrderRepository : Repository<OrderEntity>, IOrderRepository
    {
        public OrderRepository(OrderDbContext db) : base(db)
        { }
    }

    public class OrderItemRepository : Repository<OrderItemEntity>, IOrderItemRepository
    {
        public OrderItemRepository(OrderDbContext db) : base(db)
        { }
    }
}
