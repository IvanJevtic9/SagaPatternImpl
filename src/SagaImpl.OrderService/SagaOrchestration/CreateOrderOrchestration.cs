using SagaImpl.Common.RabbitMQ.Sender;
using SagaImpl.Common.Saga;
using SagaImpl.OrderService.Database;
using SagaImpl.OrderService.Entities;
using System;
using System.Threading.Tasks;

namespace SagaImpl.OrderService.SagaOrchestration
{
    public class CreateOrderOrchestration : IOrchestration
    {
        private OrderEntity order;
        private readonly OrderPublisher publisher;
        private readonly UnitOfWork unitOfWork;

        public CreateOrderOrchestration(UnitOfWork unitOfWork, OrderPublisher publisher)
        {
            this.unitOfWork = unitOfWork;
            this.publisher = publisher;
        }

        public async Task StartAsync()
        {
            //this.order = new OrderEntity()
            //{
            //    UserId = userId,
            //    CreatedDate = DateTimeOffset.Now
            //};

        }
    }
}
