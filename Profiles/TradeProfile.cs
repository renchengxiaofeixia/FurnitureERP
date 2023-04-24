
namespace FurnitureERP.Profiles
{
    public class TradeProfile : Profile
    {
        public TradeProfile()
        {
            CreateMap<CreateTradeDto, Trade>()
                .ReverseMap();

            CreateMap<TradeDto, Trade>()
                .ReverseMap();

            CreateMap<TradeItemDto, TradeItem>()
                .ReverseMap();

            CreateMap<CreateTradeItemDto, TradeItem>() 
                .ReverseMap();

        }
    }
}
