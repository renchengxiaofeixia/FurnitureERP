
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

            CreateMap<TradeItemDto, TradePackage>()
                .ForMember(dest => dest.PackageName, opt => opt.MapFrom(src => src.ItemName))
                .ForMember(dest => dest.PackageNo, opt => opt.MapFrom(src => src.ItemNo))
                .ForMember(dest => dest.StdPackageNo, opt => opt.MapFrom(src => src.StdItemNo))
                .ReverseMap()
                .ForPath(src => src.ItemName, opt => opt.MapFrom(dest => dest.PackageName))
                .ForPath(src => src.ItemNo, opt => opt.MapFrom(dest => dest.PackageNo))
                .ForPath(src => src.StdItemNo, opt => opt.MapFrom(dest => dest.StdPackageNo));

            CreateMap<CreateTradeItemDto, TradePackage>()
                .ForMember(dest => dest.PackageName, opt => opt.MapFrom(src => src.ItemName))
                .ForMember(dest => dest.PackageNo, opt => opt.MapFrom(src => src.ItemNo))
                .ForMember(dest => dest.StdPackageNo, opt => opt.MapFrom(src => src.StdItemNo))
                .ReverseMap()
                .ForPath(src => src.ItemName, opt => opt.MapFrom(dest => dest.PackageName))
                .ForPath(src => src.ItemNo, opt => opt.MapFrom(dest => dest.PackageNo))
                .ForPath(src => src.StdItemNo, opt => opt.MapFrom(dest => dest.StdPackageNo));
        }
    }
}
