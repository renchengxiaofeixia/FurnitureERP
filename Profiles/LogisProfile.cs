
using FurnitureERP.Dtos;

namespace FurnitureERP.Profiles
{
    public class LogisProfile : Profile
    {
        public LogisProfile()
        {
            CreateMap<CreateLogisticDto, Logistic>()
            .ReverseMap();

            CreateMap<LogisticDto, Logistic>()
                .ReverseMap();

            CreateMap<CreateLogisPointDto, LogisPoint>()
                .ReverseMap();

            CreateMap<LogisPointDto, LogisPoint>()
                .ReverseMap();
        }
    }
}
