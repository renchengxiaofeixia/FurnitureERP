
namespace FurnitureERP.Profiles
{
    public class PropertyConfigProfile : Profile
    {
        public PropertyConfigProfile()
        {
            CreateMap<CreatePropertyConfigDto, PropertyConfig>()
                .ForMember(dest => dest.PropertyConfigJson, opt => opt.MapFrom(src => JsonSerializer.Serialize(src.Properties,new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = false,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true,
                })))
                .ReverseMap()
                .ForPath(src => src.Properties, opt => opt.MapFrom(dest => JsonSerializer.Deserialize<List<Property>>(dest.PropertyConfigJson,new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = false,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true,
                })));


            CreateMap<PropertyConfigDto, PropertyConfig>()
                .ForMember(dest => dest.PropertyConfigJson, opt => opt.MapFrom(src => JsonSerializer.Serialize(src.Properties, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = false,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true,
                })))
                .ReverseMap()
                .ForPath(src => src.Properties, opt => opt.MapFrom(dest => JsonSerializer.Deserialize<List<Property>>(dest.PropertyConfigJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = false,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true,
                })));

        }
    }
}
