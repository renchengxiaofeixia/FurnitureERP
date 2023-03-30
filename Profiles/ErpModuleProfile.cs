
namespace FurnitureERP.Profiles
{
    public class ErpModuleProfile : Profile
    {
        public ErpModuleProfile()
        {
            CreateMap<ErpModuleDto, ErpModule>()
                .ReverseMap();

        }
    }
}
