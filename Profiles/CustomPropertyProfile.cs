
namespace FurnitureERP.Profiles
{
    public class CustomPropertyProfile : Profile
    {
        public CustomPropertyProfile()
        {
            CreateMap<CustomPropertyDto, CustomProperty>()
                .ReverseMap();

        }
    }
}
