using Microsoft.AspNetCore.Mvc;
using Saga.Common.Controller;
using Saga.Common.Saga.Abstraction;
using Saga.Order.Dtos;
using Saga.Order.Saga;

namespace Saga.Order.Controller
{
    public class OrderController : BaseController
    {
        private readonly IOrchestration _orchestration;
        public OrderController(IOrchestration orchestration, ISaga createOrderSaga)
        {
            _orchestration = orchestration;
            _orchestration.SetSaga(createOrderSaga);
        }

        [HttpPost()]
        public async Task<IActionResult> CreatePaymentOrder(CreateOrderRequest request, CancellationToken cancellationToken)
        {
            await _orchestration.AddContextData(nameof(CreateOrderRequest), request);
            
            await _orchestration.Start();

            return Ok();
        }
    }
}
