using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SagaImpl.Common.ModelDtos;
using SagaImpl.InventoryService.Database;
using SagaImpl.InventoryService.MediatR.Commands;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SagaImpl.InventoryService.MediatR.Handlers
{
    public class ReserveItemsHandler : IRequestHandler<ReserveItemsCommand, ReserveItemsResponseDto>
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

        public ReserveItemsHandler(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<ReserveItemsResponseDto> Handle(ReserveItemsCommand request, CancellationToken cancellationToken)
        {
            var scope = serviceScopeFactory.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<UnitOfWork>();

            var res = new ReserveItemsResponseDto()
            {
                Items = new List<ReservedItemsDto>()
            };

            await semaphore.WaitAsync();
            foreach (var item in request.Items)
            {
                var product = await unitOfWork.Product.GetByIdAsync(item.ItemId);

                if (product == null || product.Quantity < item.NumberOf)
                {
                    semaphore.Release();
                    int qunt = product == null ? 0 : product.Quantity;

                    res.Items.Clear();
                    res.IsSuccess = false;
                    res.Message = $"Invalid reservation for item id: {item.ItemId}, number of: {item.NumberOf} (Available - {qunt})";
                    
                    return res;
                }

                res.Items.Add(new ReservedItemsDto
                {
                    NumberOf = item.NumberOf,
                    Price = product.Price,
                    DisplayMessage = product.Description
                });

                product.Quantity -= item.NumberOf;
            }

            await unitOfWork.SaveChangesAsync();
            semaphore.Release();

            res.IsSuccess = true;
            res.Message = $"All items has been reserved.";

            return res;
        }
    }
}
