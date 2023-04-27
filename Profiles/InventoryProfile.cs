namespace FurnitureERP.Profiles
{
    public class InventoryProfile : Profile
    {
        public InventoryProfile()
        {
            CreateMap<InventoryItemAdjustDto, InventoryItemAdjust>()
                .ReverseMap();

            CreateMap<InventoryItemMoveDto, InventoryItemMove>()
                .ReverseMap();

            CreateMap<InventoryItemMoveDto, Inventory>()
                .ReverseMap();

            CreateMap<InventoryItemMove, InventoryPackage>()
                .ReverseMap();

            CreateMap<InventoryItemAdjust,InventoryPackage>()
                .ForMember(dest => dest.PackageName, opt => opt.MapFrom(src => src.ItemName))
                .ForMember(dest => dest.PackageNo, opt => opt.MapFrom(src => src.ItemNo))
                .ReverseMap()
                .ForPath(src => src.ItemName, opt => opt.MapFrom(dest => dest.PackageName))
                .ForPath(src => src.ItemNo, opt => opt.MapFrom(dest => dest.PackageNo));

        }
    }
}
