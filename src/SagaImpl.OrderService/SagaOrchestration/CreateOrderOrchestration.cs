using System.Text.Json;
using SagaImpl.Common;
using SagaImpl.Common.RabbitMQ.Sender;
using SagaImpl.Common.Saga;
using SagaImpl.Common.Saga.Abstaction;
using SagaImpl.OrderService.Database;
using SagaImpl.OrderService.Entities;
using SagaImpl.OrderService.Models.Request;
using System.Collections.Generic;
using System.Threading.Tasks;
using SagaImpl.Common.Saga.Enums;
using SagaImpl.Common.ModelDtos;
using AutoMapper;
using System.Text;
using SagaImpl.Common.Extension;
using SagaImpl.OrderService.Messaging.Receiver;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client.Events;
using System;
using SagaImpl.OrderService.Models;

namespace SagaImpl.OrderService.SagaOrchestration
{
    public class CreateOrderOrchestration : IOrchestration
    {
        private UnitOfWork unitOfWork;
        private readonly OrderPublisher publisher;
        private readonly OrchestratorSubscriber subscriber;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly IMapper mapper;

        private OrderEntity order;
        private SagaSession session;

        public bool IsAlive { get; private set; } = false;

        public EventHandler<OrchestrationEventArgs> AcknowladgeReceivedMessage;

        public EventHandler<BasicDeliverEventArgs> Handler;

        public CreateOrderOrchestration(UnitOfWork unitOfWork, OrderPublisher publisher, IMapper mapper, IServiceScopeFactory serviceScopeFactory, OrchestratorSubscriber subscriber)
        {
            this.unitOfWork = unitOfWork;
            this.publisher = publisher;
            this.mapper = mapper;
            this.serviceScopeFactory = serviceScopeFactory;
            this.subscriber = subscriber;

            Handler = new EventHandler<BasicDeliverEventArgs>(onMessageReceived);

            subscriber.SubscribeOnChannel(Handler, this);
        }

        public async Task StartAsync(object input)
        {
            if (!IsAlive)
            {
                IsAlive = true;

                var req = (CreateOrderRequest)input;

                session = new SagaSession
                {
                    Status = SagaStatus.Running.ToString(),
                    Logs = new List<SagaLog>()
                };

                session.Logs.Add(new SagaLog
                {
                    Session = session,
                    Name = CreateOrderSagaEvents.StartSaga.ToString(),
                    LogTypeId = (int)LType.Start
                });

                session.Logs.Add(new SagaLog
                {
                    Session = session,
                    Name = CreateOrderSagaEvents.CreateOrder.ToString(),
                    LogTypeId = (int)LType.Start,
                    Log = JsonSerializer.Serialize(req)
                });

                order = new OrderEntity
                {
                    UserId = req.UserId
                };

                await unitOfWork.Order.AddAsync(order);
                await unitOfWork.SaveChangesAsync();

                session.Logs.Add(new SagaLog
                {
                    Session = session,
                    Name = CreateOrderSagaEvents.CreateOrder.ToString(),
                    LogTypeId = (int)LType.End,
                    Log = JsonSerializer.Serialize(new { order.Id })
                });

                await unitOfWork.SagaSession.AddAsync(session);
                await unitOfWork.SaveChangesAsync();

                var items = JsonSerializer.Serialize(req.Items);
                var reserveItems = new Dictionary<string, object>
                {
                    { "commandName", (byte)CreateOrderSagaCommand.ReserveItems},
                    { "sessionId", session.Id },
                    { "orderList", items }
                };

                publisher.Publish($"Reserve items for order. Order id: {order.Id}", CommonConstants.RESERVE_ITEMS_COMMAND, reserveItems);

                session.Logs.Add(new SagaLog
                {
                    Session = session,
                    Name = CreateOrderSagaEvents.ReserveItems.ToString(),
                    LogTypeId = (int)LType.Start,
                    Log = items
                });
                await unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<bool> HandleMessage(string message, IDictionary<string, object> messageAttributes)
        {
            var scope = serviceScopeFactory.CreateScope();
            unitOfWork = scope.ServiceProvider.GetRequiredService<UnitOfWork>();
            unitOfWork.Order.Attach(order);
            unitOfWork.SagaSession.Attach(session);

            var sessionId = (int)messageAttributes["sessionId"];
            if (sessionId != session.Id) return false;

            if (messageAttributes.ContainsKey("eventName"))
            {
                CreateOrderSagaEvents nEvent = (CreateOrderSagaEvents)messageAttributes["eventName"];
                LType type = (LType)messageAttributes["eventType"];

                switch (nEvent)
                {
                    // Refactor - use MediatR for handling flow
                    case CreateOrderSagaEvents.ReserveItems:
                        switch (type)
                        {
                            case LType.End:
                                var response = ((byte[])messageAttributes["body"]).GetString();

                                session.Logs.Add(new SagaLog
                                {
                                    Session = session,
                                    Name = CreateOrderSagaEvents.ReserveItems.ToString(),
                                    LogTypeId = (int)type,
                                    Log = response
                                });

                                var body = JsonSerializer.Deserialize<List<ReservedItemsDto>>(response);
                                var orderItems = mapper.Map<List<OrderItemEntity>>(body);

                                order.AddItems(orderItems);

                                await unitOfWork.SaveChangesAsync();

                                //Sent Payment Command
                                break;
                            case LType.Abort:
                                session.Logs.Add(new SagaLog
                                {
                                    Session = session,
                                    Name = CreateOrderSagaEvents.ReserveItems.ToString(),
                                    LogTypeId = (int)type,
                                    Log = JsonSerializer.Serialize(new { message })
                                });

                                session.Logs.Add(new SagaLog
                                {
                                    Session = session,
                                    Name = CreateOrderSagaEvents.CreateOrder.ToString(),
                                    LogTypeId = (int)LType.Compesation,
                                    Log = JsonSerializer.Serialize(new { order.Id })
                                });

                                session.Logs.Add(new SagaLog
                                {
                                    Session = session,
                                    Name = CreateOrderSagaEvents.StartSaga.ToString(),
                                    LogTypeId = (int)LType.End
                                });

                                order.StatusId = (int)OrderStatusType.REJECTED;
                                session.Status = SagaStatus.Finished.ToString();

                                subscriber.UnsubscribeFromChannel(Handler);
                                await unitOfWork.SaveChangesAsync();

                                break;
                        }
                        break;
                        // Handle Payment request
                }
            }

            return true;
        }

        

        private async void onMessageReceived(object sender, BasicDeliverEventArgs e)
        {
            var body = e.Body.ToArray();
            var message = body.GetString();

            bool success = await HandleMessage(message, e.BasicProperties.Headers);

            if (success) onAcknowladgeReceivedMessage(e.DeliveryTag);
        }

        private void onAcknowladgeReceivedMessage(ulong deliveryTag)
        {
            if (AcknowladgeReceivedMessage != null) AcknowladgeReceivedMessage(this, new OrchestrationEventArgs { DeliveryTag = deliveryTag });
        }
    }
}
