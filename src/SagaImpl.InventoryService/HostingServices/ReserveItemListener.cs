using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SagaImpl.Common;
using SagaImpl.Common.Extension;
using SagaImpl.Common.ModelDtos;
using SagaImpl.Common.Saga.Enums;
using SagaImpl.InventoryService.Database;
using SagaImpl.InventoryService.Entities;
using SagaImpl.InventoryService.MediatR.Commands;
using SagaImpl.InventoryService.Messaging.Receiver;
using SagaImpl.InventoryService.Messaging.Sender;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SagaImpl.InventoryService.HostingServices
{
    public class ReserveItemListener : IHostedService
    {
        private readonly ReserveItemSubscriber subscriber;
        private readonly InventoryPublisher publisher;
        private readonly IMediator mediator;
        private readonly UnitOfWork unitOfWork;

        public ReserveItemListener(IServiceScopeFactory serviceScopeFactory)
        {
            var scope = serviceScopeFactory.CreateScope();

            subscriber = scope.ServiceProvider.GetRequiredService<ReserveItemSubscriber>();
            publisher = scope.ServiceProvider.GetRequiredService<InventoryPublisher>();
            unitOfWork = scope.ServiceProvider.GetRequiredService<UnitOfWork>();
            mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            subscriber.SubscribeAsync(Subscribe);

            if ((await unitOfWork.Product.GetFirstOrDefaultAsync()) == null)
            {
                await ProductBulkInsert();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        private async Task ProductBulkInsert()
        {
            await unitOfWork.Product.AddRangeAsync(new List<ProductEntity>
            {
                new ProductEntity
                {
                    Name = "Programiranje 1",
                    Price = 1000,
                    Quantity = 100,
                    Description = "Knjiga - I semestar"
                },
                new ProductEntity
                {
                    Name = "Programiranje 2",
                    Price = 1000,
                    Quantity = 100,
                    Description = "Knjiga - II semestar"
                },
                new ProductEntity
                {
                    Name = "UOR",
                    Price = 2200,
                    Quantity = 40,
                    Description = "Knjiga - I semestar"
                },
                new ProductEntity
                {
                    Name = "UAR",
                    Price = 1500,
                    Quantity = 30,
                    Description = "Knjiga - II semestar"
                },
                new ProductEntity
                {
                    Name = "KIIA",
                    Price = 700,
                    Quantity = 20,
                    Description = "Knjiga - II semestar"
                },
                new ProductEntity
                {
                    Name = "AISP",
                    Price = 1500,
                    Quantity = 100,
                    Description = "Knjiga - I semestar"
                },
                new ProductEntity
                {
                    Name = "UNM",
                    Price = 1000,
                    Quantity = 100,
                    Description = "Knjiga - V semestar"
                },
                new ProductEntity
                {
                    Name = "VIS",
                    Price = 600,
                    Quantity = 100,
                    Description = "Knjiga - VI semestar"
                },
                new ProductEntity
                {
                    Name = "A1",
                    Price = 900,
                    Quantity = 120,
                    Description = "Knjiga - II semestar"
                },
                new ProductEntity
                {
                    Name = "A2",
                    Price = 990,
                    Quantity = 150,
                    Description = "Knjiga - III semestar"
                },
                new ProductEntity
                {
                    Name = "DS1",
                    Price = 500,
                    Quantity = 80,
                    Description = "Knjiga - I semestar"
                },
                new ProductEntity
                {
                    Name = "LA",
                    Price = 500,
                    Quantity = 100,
                    Description = "Knjiga - I semestar "
                },
                new ProductEntity
                {
                    Name = "DS2",
                    Price = 700,
                    Quantity = 100,
                    Description = "Knjiga - II semestar"
                },
                new ProductEntity
                {
                    Name = "RG",
                    Price = 1300,
                    Quantity = 100,
                    Description = "Knjiga - V semestar"
                },
                new ProductEntity
                {
                    Name = "OOP",
                    Price = 700,
                    Quantity = 200,
                    Description = "Knjiga - IV semestar"
                }
            });
            await unitOfWork.SaveChangesAsync();
        }

        private async Task<bool> Subscribe(string message, IDictionary<string, object> messageAttributes)
        {
            CreateOrderSagaCommand command = (CreateOrderSagaCommand)messageAttributes["commandName"];
            switch (command)
            {
                case CreateOrderSagaCommand.ReserveItems:
                    var commandBody = ((byte[])messageAttributes["orderList"]).GetString();
                    var items = JsonSerializer.Deserialize<List<OrderItemDto>>(commandBody);
                    var response = await mediator.Send(new ReserveItemsCommand { Items = items });

                    if (response.IsSuccess)
                    {
                        var reservedItems = JsonSerializer.Serialize(response.Items);
                        var eventAttributes = new Dictionary<string, object>()
                            {
                                { "eventName",  (byte)CreateOrderSagaEvents.ReserveItems },
                                { "sessionId", messageAttributes["sessionId"] },
                                { "eventType", (byte)LType.End },
                                { "body", reservedItems }
                            };

                        publisher.Publish(response.Message, CommonConstants.CREATE_ORDER_ORCHESTRATION, eventAttributes);
                    }
                    else
                    {
                        var eventAttributes = new Dictionary<string, object>()
                            {
                                { "eventName",  (byte)CreateOrderSagaEvents.ReserveItems },
                                { "sessionId", messageAttributes["sessionId"] },
                                { "eventType", (byte)LType.Abort }
                            };

                        publisher.Publish(response.Message, CommonConstants.CREATE_ORDER_ORCHESTRATION, eventAttributes);
                    }

                    break;
                    // Implement compesation command
            }

            return true;
        }
    }
}
