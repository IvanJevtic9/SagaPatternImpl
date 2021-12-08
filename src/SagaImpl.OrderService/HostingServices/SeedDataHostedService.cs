using Microsoft.Extensions.Hosting;
using SagaImpl.Common.Abstraction.Interface;
using SagaImpl.Common.Saga;
using SagaImpl.OrderService;
using SagaImpl.OrderService.Database;
using SagaImpl.OrderService.SagaOrchestration;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SagaImpl.InventoryService.HostingServices
{
    public class SeedDataHostedService : IHostedService
    {
        private readonly ILoggerAdapter<SeedDataHostedService> logger;
        private readonly UnitOfWork unitOfWork;

        public SeedDataHostedService(ILoggerAdapter<SeedDataHostedService> logger, UnitOfWork unitOfWork)
        {
            this.logger = logger;
            this.unitOfWork = unitOfWork;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await CreateOrderSaga();
        }

        public async Task StopAsync(CancellationToken cancellationToken) => await Task.CompletedTask;

        public async Task CreateOrderSaga()
        {
            var exist = await unitOfWork.SagaDefinition.AnyAsync(s => s.Name == Consents.SagaNames.CREATE_ORDER);

            if (!exist)
            {
                var saga = new SagaDefinition
                {
                    Name = Consents.SagaNames.CREATE_ORDER,
                    NumberOfPhases = 4,
                    Steps = new List<SagaStep>()
                };

                saga.Steps.Add(new SagaStep
                {
                    Definition = saga,
                    Phase = 1,
                    TransactionMethod = CreateOrderSagaCommand.CreateOrder.ToString(),
                    CompensationMethod = CreateOrderSagaCommand.RejectOrder.ToString()
                });
                saga.Steps.Add(new SagaStep
                {
                    Definition = saga,
                    Phase = 2,
                    TransactionMethod = CreateOrderSagaCommand.ReserveItems.ToString(),
                    CompensationMethod = CreateOrderSagaCommand.UnreserveItems.ToString()
                });
                saga.Steps.Add(new SagaStep
                {
                    Definition = saga,
                    Phase = 3,
                    TransactionMethod = CreateOrderSagaCommand.PayOrder.ToString(),
                    CompensationMethod = CreateOrderSagaCommand.RefundMoney.ToString()
                });
                saga.Steps.Add(new SagaStep
                {
                    Definition = saga,
                    Phase = 4,
                    TransactionMethod = CreateOrderSagaCommand.FinishSaga.ToString(),
                });

                await unitOfWork.SagaDefinition.AddAsync(saga);
            }
        }
    }
}
