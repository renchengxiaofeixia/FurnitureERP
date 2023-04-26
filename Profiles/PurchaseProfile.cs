
namespace FurnitureERP.Profiles
{
    public class PurchaseProfile : Profile
    {
        public PurchaseProfile()
        {
            CreateMap<CreatePurchaseDto, Purchase>()
                .ReverseMap();

            CreateMap<PurchaseDto, Purchase>()
                .ReverseMap();

            CreateMap<PurchaseItemDto, PurchaseItem>()
                .ReverseMap();

            CreateMap<CreatePurchaseItemDto, PurchaseItem>() 
                .ReverseMap();

            CreateMap<PurchaseItemDto, PurchasePackage>()
                .ForMember(dest => dest.PackageName, opt => opt.MapFrom(src =>src.ItemName ))
                .ForMember(dest => dest.PackageNo, opt => opt.MapFrom(src => src.ItemNo))
                .ForMember(dest => dest.StdPackageNo, opt => opt.MapFrom(src => src.StdItemNo))
                .ReverseMap()
                .ForPath(src => src.ItemName, opt => opt.MapFrom(dest =>  dest.PackageName))
                .ForPath(src => src.ItemNo, opt => opt.MapFrom(dest => dest.PackageNo))
                .ForPath(src => src.StdItemNo, opt => opt.MapFrom(dest => dest.StdPackageNo));

            CreateMap<CreatePurchaseItemDto, PurchasePackage>()
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
