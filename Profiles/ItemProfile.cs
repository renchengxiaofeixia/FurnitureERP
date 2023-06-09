﻿
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

            CreateMap<PackageDto, Package>()
                .ReverseMap();

            CreateMap<CreatePackageDto, Package>()
                .ReverseMap();

            CreateMap<ItemCatDto, ItemCat>()
                .ReverseMap();

            CreateMap<CreateItemCatDto, ItemCat>()
                .ReverseMap();
        }
    }
}
