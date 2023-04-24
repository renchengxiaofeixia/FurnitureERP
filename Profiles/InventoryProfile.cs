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

        }
    }
}
