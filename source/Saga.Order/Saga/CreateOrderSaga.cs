using FluentValidation;
using Saga.Common.Messaging.Abstraction;
using Saga.Common.Saga.Abstraction;
using Saga.Order.Data;
using Saga.Order.Dtos;
using Saga.Order.Messaging;
using Saga.Order.Saga.Steps;

namespace Saga.Order.Saga
{
    public class CreateOrderSaga : ISaga
    {
        public string Name => GetType().Name;
        public List<(int Level, ISagaStep Step)> Steps { get; } = new();

        public CreateOrderSaga(IEventProcessor eventProcessor, UnitOfWork unitOfWork, IValidator<CreateOrderRequest> createOrderValidator, OrderPublisher orderPublisher)
        {
            Steps.Add((1, new CreateOrderStep(createOrderValidator, eventProcessor, unitOfWork)));
            Steps.Add((2, new ReserveItemsStep(unitOfWork, orderPublisher)));
        }
    }
}

