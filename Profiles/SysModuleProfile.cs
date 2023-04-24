
namespace FurnitureERP.Profiles
{
    public class SysModuleProfile : Profile
    {
        public SysModuleProfile()
        {
            CreateMap<SysModuleDto, SysModule>()
                .ReverseMap();

        }
    }
}
