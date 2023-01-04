using Microsoft.EntityFrameworkCore;
using Saga.Common.Data;
using Saga.Order.Data.Repository.Interface;
using Saga.Order.Entities;

namespace Saga.Order.Data.Repository
{
    public class PaymentOrderRepository : Repository<PaymentOrder>, IPaymentOrderRepository
    {
        public PaymentOrderRepository(DbContext db) : base(db)
        { }
    }
}
