using SagaImpl.Common.ModelDtos;
using System.Collections.Generic;

namespace SagaImpl.OrderService.Models.Request
{
    public class CreateOrderRequest
    {
        public int UserId { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }
}
