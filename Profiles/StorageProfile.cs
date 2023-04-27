namespace FurnitureERP.Profiles
{
    public class StorageProfile : Profile
    {
        public StorageProfile()
        {
            CreateMap<CreateStorageDto, Storage>()
                .ReverseMap();

            CreateMap<StorageDto, Storage>()
                .ReverseMap();

            CreateMap<StorageItemDto, StorageItem>()
                .ReverseMap();

            CreateMap<CreateStorageItemDto, StorageItem>()
                .ReverseMap();

            CreateMap<StorageItem, Inventory>()
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.StorageNum))
                .ReverseMap()
                .ForPath(src => src.StorageNum, opt => opt.MapFrom(dest => dest.Quantity));

            CreateMap<StoragePackage, InventoryPackage>()
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.StorageNum))
                .ReverseMap()
                .ForPath(src => src.StorageNum, opt => opt.MapFrom(dest => dest.Quantity));

            CreateMap<StorageItemDto, PurchasePackage>()
                .ForMember(dest => dest.PackageName, opt => opt.MapFrom(src => src.ItemName))
                .ForMember(dest => dest.PackageNo, opt => opt.MapFrom(src => src.ItemNo))
                .ForMember(dest => dest.StdPackageNo, opt => opt.MapFrom(src => src.StdItemNo))
                .ReverseMap()
                .ForPath(src => src.ItemName, opt => opt.MapFrom(dest => dest.PackageName))
                .ForPath(src => src.ItemNo, opt => opt.MapFrom(dest => dest.PackageNo))
                .ForPath(src => src.StdItemNo, opt => opt.MapFrom(dest => dest.StdPackageNo));

            CreateMap<CreateStorageItemDto, PurchasePackage>()
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
