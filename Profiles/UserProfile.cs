
namespace MiniErp.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<CreateUserDto, User>()
                .ReverseMap();

            CreateMap<UserDto, User>()
                .ReverseMap();
        }
    }
}
