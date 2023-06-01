
using FurnitureERP.Enums;

namespace FurnitureERP.Profiles
{
    public class PurchaseProfile : Profile
    {
        public PurchaseProfile()
        {
            CreateMap<CreatePurchaseDto, Purchase>()
                .ReverseMap();

            CreateMap<PurchaseDto, Purchase>()
                //.ForMember(dest => dest.SettlementMode, opt => opt.MapFrom(src => src.SettlementMode.ToString()))
                .ReverseMap();
                //.ForPath(src => src.SettlementMode, opt => opt.MapFrom(dest => Convert.ChangeType(Enum.Parse(typeof(SettlementModeEnum), dest.SettlementMode), typeof(SettlementModeEnum))));

            CreateMap<PurchaseItemDto, PurchaseItem>()
                .ReverseMap();

            CreateMap<CreatePurchaseItemDto, PurchaseItem>() 
                .ReverseMap();

            //CreateMap<PurchaseItemDto, PurchasePackage>()
            //    .ForMember(dest => dest.PackageName, opt => opt.MapFrom(src =>src.ItemName ))
            //    .ForMember(dest => dest.PackageNo, opt => opt.MapFrom(src => src.ItemNo))
            //    .ForMember(dest => dest.StdPackageNo, opt => opt.MapFrom(src => src.StdItemNo))
            //    .ReverseMap()
            //    .ForPath(src => src.ItemName, opt => opt.MapFrom(dest =>  dest.PackageName))
            //    .ForPath(src => src.ItemNo, opt => opt.MapFrom(dest => dest.PackageNo))
            //    .ForPath(src => src.StdItemNo, opt => opt.MapFrom(dest => dest.StdPackageNo));

            //CreateMap<CreatePurchaseItemDto, PurchasePackage>()
            //    .ForMember(dest => dest.PackageName, opt => opt.MapFrom(src => src.ItemName))
            //    .ForMember(dest => dest.PackageNo, opt => opt.MapFrom(src => src.ItemNo))
            //    .ForMember(dest => dest.StdPackageNo, opt => opt.MapFrom(src => src.StdItemNo))
            //    .ReverseMap()
            //    .ForPath(src => src.ItemName, opt => opt.MapFrom(dest => dest.PackageName))
            //    .ForPath(src => src.ItemNo, opt => opt.MapFrom(dest => dest.PackageNo))
            //    .ForPath(src => src.StdItemNo, opt => opt.MapFrom(dest => dest.StdPackageNo));
        }
    }
}
