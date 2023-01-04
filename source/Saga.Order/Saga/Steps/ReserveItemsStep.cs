using Saga.Common;
using Saga.Common.Entities.Saga;
using Saga.Common.Messaging.Abstraction;
using Saga.Common.Saga.Abstraction;
using Saga.Common.SharedDtos;
using Saga.Order.Data;
using Saga.Order.Dtos;
using Saga.Order.Messaging;
using System.Text.Json;

namespace Saga.Order.Saga.Steps
{
    public class ReserveItemsStep : ISagaStep
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly OrderPublisher _publisher;

        private int _sessionId;
        private IDictionary<string, object> _context;

        public ReserveItemsStep(UnitOfWork unitOfWork, OrderPublisher inventoryPublisher)
        {
            _unitOfWork = unitOfWork;
            _publisher = inventoryPublisher;
        }
        public string CorrelationId { get; private set; }

        public string StepName => GetType().Name;

        public async Task SetContext(int sessionId, string correlationId, IDictionary<string, object> context)
        {
            CorrelationId = correlationId;
            _context = context;
            _sessionId = sessionId;
        }

        public async Task Commit()
        {
            var orderContext = (PaymentOrderDto)_context["PaymentOrder"];
            var paymentOrder = await _unitOfWork.PaymentOrder.GetFirstOrDefaultAsync(x => x.Id == orderContext.OrderId, "Items");

            var request = new ReserveItemsRequest()
            {
                CorrelationId = CorrelationId
            };

            foreach (var item in paymentOrder.Items)
            {
                request.Items.Add(new()
                {
                    ProductId = item.ProductId,
                    Quantity = item.NumberOf,
                    Price = item.Price
                });
            }

            var log = new SagaLog()
            {
                Step = StepName,
                SessionId = _sessionId,
                Log = JsonSerializer.Serialize(request),
                TypeId = LogType.Start,
                Type = LogType.Start.ToString()
            };

            await _unitOfWork.SagaLog.AddAsync(log);
            await _unitOfWork.SaveChangesAsync();

            var messageAttributes = new Dictionary<string, object>();
            messageAttributes.Add("command", "ReserveItems");

            _publisher.Publish(JsonSerializer.Serialize(request), "Inventory.ReserveItem", messageAttributes);
        }

        public Task Complete() => Task.CompletedTask;

        public async Task RollBack()
        {
            var orderContext = (PaymentOrderDto)_context["PaymentOrder"];
            var paymentOrder = await _unitOfWork.PaymentOrder.GetFirstOrDefaultAsync(x => x.Id == orderContext.OrderId, "Items");

            var request = new ReserveItemsRequest()
            {
                CorrelationId = CorrelationId,
                UndoOperation = true
            };

            foreach (var item in paymentOrder.Items)
            {
                request.Items.Add(new()
                {
                    ProductId = item.ProductId,
                    Quantity = item.NumberOf,
                    Price = item.Price
                });
            }

            var log = new SagaLog()
            {
                Step = StepName,
                SessionId = _sessionId,
                Log = JsonSerializer.Serialize(request),
                TypeId = LogType.Compesation,
                Type = LogType.Compesation.ToString()
            };

            await _unitOfWork.SagaLog.AddAsync(log);
            await _unitOfWork.SaveChangesAsync();

            var messageAttributes = new Dictionary<string, object>();
            messageAttributes.Add("command", "ReserveItems");

            _publisher.Publish(JsonSerializer.Serialize(request), "Inventory.ReserveItem", messageAttributes);
        }
    }
}