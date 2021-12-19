using MediatR;
using SagaImpl.Common.ModelDtos;
using System.Collections.Generic;

namespace SagaImpl.InventoryService.MediatR.Commands
{
    public class ReserveItemsCommand : IRequest<ReserveItemsResponseDto>
    {
        public List<OrderItemDto> Items { get; set; }
    }
}
