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
        }
    }
}
