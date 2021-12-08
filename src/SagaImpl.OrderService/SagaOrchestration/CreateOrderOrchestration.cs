using SagaImpl.Common.Messaging;
using SagaImpl.Common.RabbitMQ.Sender;
using SagaImpl.Common.Saga;
using SagaImpl.Common.Saga.Abstaction;
using SagaImpl.OrderService.Database;
using SagaImpl.OrderService.Entities;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SagaImpl.OrderService.SagaOrchestration
{
    public enum CreateOrderSagaCommand : byte
    {
        CreateOrder,
        RejectOrder,
        ReserveItems,
        UnreserveItems,
        PayOrder,
        RefundMoney,
        FinishSaga
    }

    public enum CreateOrderSagaEvent : byte
    {

    }

    public class CreateOrderOrchestration : IOrchestration
    {
        private readonly UnitOfWork unitOfWork;
        
        private OrderEntity order;
        private readonly OrderPublisher publisher;

        private SagaSession session;
        private readonly SagaDefinition definition;

        private ConcurrentBag<CreateOrderSagaEvent> eventQueue = new ConcurrentBag<CreateOrderSagaEvent>();

        public bool IsAlive { get; private set; } = false;

        public CreateOrderOrchestration(UnitOfWork unitOfWork, OrderPublisher publisher)
        {
            this.unitOfWork = unitOfWork;
            this.publisher = publisher;

            definition = loadDefinition(Consents.OrchestrationNames.CREATE_ORDER);
        }

        public async Task StartAsync()
        {
            session = new SagaSession
            {
                SagaDefinitionId = definition.Id,
                SagaDefinition = definition,
                //Status = 
                Logs = new List<SagaLog>()
            };

            //this.order = new OrderEntity()
            //{
            //    UserId = userId,
            //    CreatedDate = DateTimeOffset.Now
            //};
        }

        public async Task ContinueAsync()
        {

        }

        public void OnMessageReceive(string message, IDictionary<string, object> messageAttributes)
        {
            throw new System.NotImplementedException();
        }

        private SagaDefinition loadDefinition(string name)
        {
            return unitOfWork.SagaDefinition.GetFirstOrDefaultAsync(a => a.Name == name).Result;
        }
    }
}
