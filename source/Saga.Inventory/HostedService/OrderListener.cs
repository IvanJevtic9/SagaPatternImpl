using Saga.Common.SharedDtos;
using Saga.Inventory.Data;
using Saga.Inventory.Messaging;
using System.Text;
using System.Text.Json;

namespace SagaImpl.InventoryService.HostingServices
{
    public class OrderListener : IHostedService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly OrderSubscriber _subscriber;
        private readonly InventoryPublisher _publisher;

        public OrderListener(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;

            var scope = serviceScopeFactory.CreateScope();

            _subscriber = scope.ServiceProvider.GetRequiredService<OrderSubscriber>();
            _publisher = scope.ServiceProvider.GetRequiredService<InventoryPublisher>();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _subscriber.SubscribeAsync(Subscribe);
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;


        private async Task<bool> Subscribe(string message, IDictionary<string, object> messageAttributes)
        {
            using var scope = _serviceScopeFactory.CreateScope();

            var unitOfWork = scope.ServiceProvider.GetRequiredService<UnitOfWork>();

            var command = Encoding.UTF8.GetString((byte[])messageAttributes["command"]);

            // We can move this part in command , also validate request.
            if (command == "ReserveItems")
            {
                var success = true;
                var responseMessage = "";

                var request = JsonSerializer.Deserialize<ReserveItemsRequest>(message);
                var productIds = request.Items.Select(x => x.ProductId).ToList();

                var products = await unitOfWork.Product.GetAllAsync(x => productIds.Contains(x.Id));

                if (!request.UndoOperation)
                {
                    foreach (var product in products)
                    {
                        var reqItem = request.Items.FirstOrDefault(x => x.ProductId == product.Id);

                        if (reqItem.Price != product.Price)
                        {
                            success = false;
                            responseMessage += $"[Price missmatch] {product.Description} - current price: {product.Price} / sent price: {reqItem.Price}\n";
                        }

                        if (reqItem.Quantity > product.Quantity)
                        {
                            success = false;
                            responseMessage += $"[Not available] {product.Description} - on balance: {product.Quantity} / asked for: {reqItem.Quantity}\n";
                        }

                        product.Quantity -= reqItem.Quantity;
                        request.Items.Remove(reqItem);
                    }

                    if (request.Items.Count > 0)
                    {
                        success = false;
                        responseMessage += $"[Not exist] Product Ids - {string.Join(',', request.Items.Select(x => x.ProductId).ToArray())}\n";
                    }

                    var attributes = new Dictionary<string, object>();
                    attributes.Add("command", "ReserveItem");
                    attributes.Add("result", success);
                    attributes.Add("correlationId", request.CorrelationId);

                    if (success)
                    {
                        responseMessage = "Products have been reserved.";
                        await unitOfWork.SaveChangesAsync();
                    }

                    _publisher.Publish(responseMessage, "Order.ReservedItem", attributes);
                }
                else
                {
                    foreach (var product in products)
                    {
                        var reqItem = request.Items.FirstOrDefault(x => x.ProductId == product.Id);

                        product.Quantity += reqItem.Quantity;
                    }

                    //TODO send response

                    await unitOfWork.SaveChangesAsync();
                }
            }

            return true;
        }
    }
}
