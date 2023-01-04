using Microsoft.EntityFrameworkCore;
using Saga.Common.Data;
using Saga.Order.Data.Repository.Interface;
using Saga.Order.Entities;

namespace Saga.Order.Data.Repository
{
    public class PaymentOrderItemRepository : Repository<PaymentOrderItem>, IPaymentOrderItemRepository
    {
        public PaymentOrderItemRepository(DbContext db) : base(db)
        { }
    }
}
