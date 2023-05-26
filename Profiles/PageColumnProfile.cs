
namespace MiniErp.Profiles
{
    public class PageColumnProfile : Profile
    {
        public PageColumnProfile()
        {
            CreateMap<CreatePageColumnDto, PageColumn>()
                .ReverseMap();

            CreateMap<PageColumnDto, PageColumn>()
                .ReverseMap();
        }
    }
}
