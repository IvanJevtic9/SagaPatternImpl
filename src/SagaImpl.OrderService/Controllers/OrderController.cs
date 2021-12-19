using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SagaImpl.Common.Apstraction.Interface;
using SagaImpl.Common.RabbitMQ.Sender;
using SagaImpl.OrderService.Database;
using SagaImpl.OrderService.Messaging.Receiver;
using SagaImpl.OrderService.Models.Request;
using SagaImpl.OrderService.SagaOrchestration;
using System.Threading.Tasks;

namespace SagaImpl.OrderService.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly UnitOfWork unitOfWork;
        private readonly OrderPublisher publisher;
        private readonly OrchestratorSubscriber subscriber;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly IMapper mapper;

        public OrderController(UnitOfWork unitOfWork, OrderPublisher publisher, OrchestratorSubscriber subscriber, IMapper mapper, IServiceScopeFactory serviceScopeFactory)
        {
            this.unitOfWork = unitOfWork;
            this.publisher = publisher;
            this.subscriber = subscriber;
            this.mapper = mapper;
            this.serviceScopeFactory = serviceScopeFactory;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            var orchestrator = new CreateOrderOrchestration(unitOfWork, publisher, mapper, serviceScopeFactory, subscriber);

            await orchestrator.StartAsync(request);

            return Ok();
        }
    }
}
