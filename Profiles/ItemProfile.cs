
namespace FurnitureERP.Profiles
{
    public class ItemProfile : Profile
    {
        public ItemProfile()
        {
            CreateMap<CreateItemDto, Item>()
                .ReverseMap();

            CreateMap<ItemDto, Item>()
                .ReverseMap();

            CreateMap<CreateSubItemDto, SubItem>()
                .ReverseMap();

            CreateMap<SubItemDto, SubItem>()
                .ReverseMap();
        }
    }
}
