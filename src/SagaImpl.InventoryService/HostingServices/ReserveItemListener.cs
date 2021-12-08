using Microsoft.Extensions.Hosting;
using SagaImpl.Common.Abstraction.Interface;
using SagaImpl.InventoryService.Messaging.Receiver;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SagaImpl.InventoryService.HostingServices
{
    public class ReserveItemListener : IHostedService
    {
        private readonly ILoggerAdapter<ReserveItemListener> logger;
        private readonly ReserveItemSubscriber subscriber;

        public ReserveItemListener(ILoggerAdapter<ReserveItemListener> logger, ReserveItemSubscriber subscriber)
        {
            this.logger = logger;
            this.subscriber = subscriber;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Reserve Item hosted service has been started.");

            subscriber.SubscribeAsync(Subscribe);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Reserve Item hosted has been shut down.");

            return Task.CompletedTask;
        }

        private Task<bool> Subscribe(string message, IDictionary<string, object> messageAttributes)
        {
            // Do subscribe logic (this method will be invoked in consumer) - Odraditi rezervaciju + polsati obavestenje sagi o ishodu operacije.
            Console.WriteLine("Reserving items");

            return Task.Run(() => {
                return true;
            });
        }
    }
}
