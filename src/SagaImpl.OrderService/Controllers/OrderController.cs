using Microsoft.AspNetCore.Mvc;
using SagaImpl.Common;
using SagaImpl.Common.RabbitMQ.Abstraction;
using SagaImpl.Common.RabbitMQ.Sender;
using SagaImpl.OrderService.Models.Request;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SagaImpl.OrderService.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderPublisher publisher;

        public OrderController(OrderPublisher orderPublisher)
        {
            publisher = orderPublisher;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            // request Validation
            // CreateSagaOrchestration.Start()
            publisher.Publish("Test message", CommonConstants.RESERVE_ITEMS_EVENT, new Dictionary<string, object>());

            return Ok();
        }
    }
}
