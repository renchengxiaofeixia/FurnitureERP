
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace FurnitureERP.Profiles
{
    public class PropertyConfigProfile : Profile
    {
        public PropertyConfigProfile()
        {
            var option = new JsonSerializerOptions {
                PropertyNameCaseInsensitive = false,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
            };
            CreateMap<CreatePropertyConfigDto, PropertyConfig>()
                .ForMember(dest => dest.PropertyConfigJson, opt => opt.MapFrom(src => JsonSerializer.Serialize(src.Properties, option)))
                .ReverseMap()
                .ForPath(src => src.Properties, opt => opt.MapFrom(dest => JsonSerializer.Deserialize<List<PropertyDto>>(dest.PropertyConfigJson, option)));

            CreateMap<PropertyConfigDto, PropertyConfig>()
                .ForMember(dest => dest.PropertyConfigJson, opt => opt.MapFrom(src => JsonSerializer.Serialize(src.Properties, option)))
                .ReverseMap()
                .ForPath(src => src.Properties, opt => opt.MapFrom(dest => JsonSerializer.Deserialize<List<PropertyDto>>(dest.PropertyConfigJson, option)));

        }
    }
}
