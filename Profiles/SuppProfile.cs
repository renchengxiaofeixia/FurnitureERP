
namespace FurnitureERP.Profiles
{
    public class SuppProfile : Profile
    {
        public SuppProfile()
        {
            CreateMap<CreateSuppDto, Supp>()
                .ReverseMap();

            CreateMap<SuppDto, Supp>()
                .ReverseMap();
        }
    }
}
