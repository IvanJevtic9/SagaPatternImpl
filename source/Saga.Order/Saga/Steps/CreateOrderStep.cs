using FluentValidation;
using Saga.Common;
using Saga.Common.Entities.Saga;
using Saga.Common.Messaging.Abstraction;
using Saga.Common.Saga.Abstraction;
using Saga.Order.Dtos;
using Saga.Order.Entities;
using Saga.Order.Data;
using System.Text.Json;

namespace Saga.Order.Saga.Steps
{
    public class CreateOrderStep : ISagaStep
    {
        private readonly IValidator<CreateOrderRequest> _creatOrderValidator;
        private readonly IEventProcessor _eventProcessor;
        private readonly UnitOfWork _unitOfWork;

        private int _sessionId;
        private IDictionary<string, object> _context;

        public CreateOrderStep(IValidator<CreateOrderRequest> validator, IEventProcessor eventProcessor, UnitOfWork unitOfWork)
        {
            _creatOrderValidator = validator;
            _eventProcessor = eventProcessor;
            _unitOfWork = unitOfWork;
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
            var request = (CreateOrderRequest)_context[nameof(CreateOrderRequest)];
            var log = new SagaLog()
            {
                Step = StepName,
                SessionId = _sessionId,
                Log = JsonSerializer.Serialize(request),
                TypeId = LogType.Start,
                Type = LogType.Start.ToString()
            };

            await _unitOfWork.SagaLog.AddAsync(log);

            var errors = _creatOrderValidator.GetErrors(request);
            if (errors.Any())
            {
                var errorLog = new SagaLog()
                {
                    Step = StepName,
                    SessionId = _sessionId,
                    TypeId = LogType.Abort,
                    Type = LogType.Abort.ToString(),
                    Log = JsonSerializer.Serialize(errors)
                };

                await _unitOfWork.SagaLog.AddAsync(errorLog);
                await _unitOfWork.SaveChangesAsync();

                var validationErrorEvent = new Event<string>()
                {
                    CorrelationId = CorrelationId,
                    Type = EventType.Failure
                };

                await _eventProcessor.ProcessEvent(validationErrorEvent);
                return;
            }

            var paymentOrder = new PaymentOrder()
            {
                UserId = request.UserId,
                Currency = request.Currency.ToString(),
                Items = new List<PaymentOrderItem>()
            };

            var productIds = request.Items.Select(x => x.ProductId).ToList();
            var products = await _unitOfWork.ProductRepository.GetAllAsync(x => productIds.Contains(x.ProductId));

            var paymentItems =
                from product in products
                join item in request.Items on product.ProductId equals item.ProductId
                select new PaymentOrderItem()
                {
                    ProductId = item.ProductId,
                    NumberOf = item.Quantity,
                    Price = product.Price
                };

            paymentOrder.AddItems(paymentItems.ToList());

            await _unitOfWork.PaymentOrder.AddAsync(paymentOrder);
            await _unitOfWork.SagaLog.AddAsync(new()
            {
                Step = StepName,
                SessionId = _sessionId,
                Log = "Order was created successfully.",
                TypeId = LogType.End,
                Type = LogType.End.ToString()
            });
            await _unitOfWork.SaveChangesAsync();

            var eventMessage = new Event<PaymentOrderDto>()
            {
                CorrelationId = CorrelationId,
                ModelKey = "PaymentOrder",
                Model = new()
                {
                    OrderId = paymentOrder.Id
                },
                Type = EventType.Success
            };

            await _eventProcessor.ProcessEvent(eventMessage);
        }

        public async Task Complete()
        {
            var res = JsonSerializer.Deserialize<PaymentOrderDto>(_context["PaymentOrder"].ToString());

            var paymentOrder = await _unitOfWork.PaymentOrder.GetFirstOrDefaultAsync(p => p.Id == res.OrderId);

            paymentOrder.OrderStatus = OrderStatusEnum.Approved.ToString();

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RollBack()
        {
            var res = JsonSerializer.Deserialize<PaymentOrderDto>((string)_context["PaymentOrder"].ToString());

            var paymentOrder = await _unitOfWork.PaymentOrder.GetFirstOrDefaultAsync(p => p.Id == res.OrderId);

            paymentOrder.OrderStatus = OrderStatusEnum.Rejected.ToString();

            var log = new SagaLog()
            {
                Step = StepName,
                SessionId = _sessionId,
                Log = "Changed Order status to rejected",
                TypeId = LogType.Compesation,
                Type = LogType.Compesation.ToString()
            };

            await _unitOfWork.SagaLog.AddAsync(log);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}