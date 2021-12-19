using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SagaImpl.Common.Saga;
using SagaImpl.Common.Saga.Enums;
using SagaImpl.OrderService.Database;
using SagaImpl.OrderService.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SagaImpl.OrderService.HostedServices
{
    public class SeedDataHostedService : IHostedService
    {
        private readonly UnitOfWork unitOfWork;

        public SeedDataHostedService(IServiceScopeFactory serviceScopeFactory)
        {
            var scope = serviceScopeFactory.CreateScope();

            unitOfWork = scope.ServiceProvider.GetRequiredService<UnitOfWork>();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            foreach (var type in Enum.GetValues(typeof(LType)))
            {
                await CreateLogTypeIfNotExistAsync(type.ToString());
            }

            foreach (var type in Enum.GetValues(typeof(OrderStatusType)))
            {
                await CreateOrderStatusIfNotExistAsync(type.ToString());
            }

            await unitOfWork.SaveChangesAsync();
        }

        public async Task StopAsync(CancellationToken cancellationToken) => await Task.CompletedTask;

        private async Task CreateLogTypeIfNotExistAsync(string typeName)
        {
            var typeExist = await unitOfWork.LogType.AnyAsync(l => l.Name == typeName);

            if (!typeExist)
            {
                var succ = Enum.TryParse(typeName, out LType type);

                if (succ) await unitOfWork.LogType.AddAsync(new LogType { Id = (int)type, Name = type.ToString() });
            }
        }

        private async Task CreateOrderStatusIfNotExistAsync(string typeName)
        {
            var typeExist = await unitOfWork.OrderStatus.AnyAsync(l => l.Name == typeName);

            if (!typeExist)
            {
                var succ = Enum.TryParse(typeName, out OrderStatusType type);

                if (succ) await unitOfWork.OrderStatus.AddAsync(new OrderStatus { Id = (int)type, Name = type.ToString() });
            }
        }
    }
}
