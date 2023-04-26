namespace FurnitureERP.Profiles
{
    public class LoginLogProfile : Profile
    {
        public LoginLogProfile() 
        {
            CreateMap<CreateLoginLogDto, LoginLog>()
                .ReverseMap();

            CreateMap<LoginLogDto, LoginLog>()
                .ReverseMap();
        }
    }
}
