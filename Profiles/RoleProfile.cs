
namespace MiniErp.Profiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<CreateRoleDto, Role>()
                .ReverseMap();

            CreateMap<RoleDto, Role>()
                .ReverseMap();
        }
    }
}
