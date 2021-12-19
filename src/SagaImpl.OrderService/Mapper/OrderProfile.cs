using AutoMapper;
using SagaImpl.Common.ModelDtos;
using SagaImpl.OrderService.Entities;

namespace SagaImpl.OrderService.Mapper
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            MapReserveItemsToOrderItems();
        }

        private void MapReserveItemsToOrderItems()
        {
            CreateMap<ReservedItemsDto, OrderItemEntity>()
                .ForMember(dest => dest.NumberOf, src => src.MapFrom(x => x.NumberOf))
                .ForMember(dest => dest.Price, src => src.MapFrom(x => x.Price))
                .ForMember(dest => dest.DisplayMessage, src => src.MapFrom(x => x.DisplayMessage));
        }
    }
}
