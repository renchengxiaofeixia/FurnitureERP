namespace FurnitureERP.Profiles
{
    public class WarehouseProfile : Profile
    {
        public WarehouseProfile()
        {
            CreateMap<WarehouseDto, Warehouse>()
                .ReverseMap();


            CreateMap<CreateWarehouseDto, Warehouse>()
                .ReverseMap();

        }
    }
}
