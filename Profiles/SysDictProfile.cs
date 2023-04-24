
namespace FurnitureERP.Profiles
{
    public class SysDictProfile : Profile
    {
        public SysDictProfile()
        {
            CreateMap<SysDictDto, SysDict>()
                .ReverseMap();

            CreateMap<SysDictValueDto, SysDictValue>()
                .ReverseMap();

        }
    }
}
