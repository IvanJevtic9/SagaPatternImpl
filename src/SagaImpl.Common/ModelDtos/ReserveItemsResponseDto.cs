using System.Collections.Generic;

namespace SagaImpl.Common.ModelDtos
{
    public class ReserveItemsResponseDto
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public List<ReservedItemsDto> Items { get; set; }
    }
}
