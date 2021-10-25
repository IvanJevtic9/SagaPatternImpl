using System.Collections.Generic;

namespace SagaImpl.Common.ModelDtos
{
    public record ReserveOrderItems(int OrderId, List<OrderItemDto> Items);
}
