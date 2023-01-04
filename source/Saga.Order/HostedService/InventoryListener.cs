using Saga.Common;
using Saga.Common.Entities.Saga;
using Saga.Common.Messaging.Abstraction;
using Saga.Order.Data;
using Saga.Order.Messaging;
using System.Text;

namespace SagaImpl.InventoryService.HostingServices
{
    public class InventoryListener : IHostedService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IEventProcessor _eventProcessor;
        private readonly InventorySubscriber _subscriber;

        public InventoryListener(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;

            var scope = serviceScopeFactory.CreateScope();

            _subscriber = scope.ServiceProvider.GetRequiredService<InventorySubscriber>();
            _eventProcessor = scope.ServiceProvider.GetRequiredService<IEventProcessor>();
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

            if(command == "ReserveItem")
            {
                var isSuccess = (bool)messageAttributes["result"];
                var correlationId = Encoding.UTF8.GetString((byte[])messageAttributes["correlationId"]);

                var session = await unitOfWork.SagaSession.GetFirstOrDefaultAsync(x => x.CorrelationId == correlationId);

                if (isSuccess)
                {
                    var log = new SagaLog()
                    {
                        SessionId = session.Id,
                        Step = "ReserveItemsStep",
                        TypeId = LogType.End,
                        Type = LogType.End.ToString(),
                        Log = message
                    };

                    await unitOfWork.SagaLog.AddAsync(log);
                    await unitOfWork.SaveChangesAsync();
                    await _eventProcessor.ProcessEvent(new Event<string>()
                    {
                        CorrelationId = correlationId,
                        Type = EventType.Success
                    });
                }
                else
                {
                    var log = new SagaLog()
                    {
                        SessionId = session.Id,
                        Step = "ReserveItemsStep",
                        TypeId = LogType.Abort,
                        Type = LogType.Abort.ToString(),
                        Log = message
                    };

                    await unitOfWork.SagaLog.AddAsync(log);
                    await unitOfWork.SaveChangesAsync();
                    await _eventProcessor.ProcessEvent(new Event<string>()
                    {
                        CorrelationId = correlationId,
                        Type = EventType.Failure
                    });
                }
            }

            return true;
        }
    }
}
