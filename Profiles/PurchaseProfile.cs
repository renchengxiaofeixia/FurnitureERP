
namespace FurnitureERP.Profiles
{
    public class PurchaseProfile : Profile
    {
        public PurchaseProfile()
        {
            CreateMap<CreatePurchaseDto, Purchase>()
                .ReverseMap();

            CreateMap<PurchaseDto, Purchase>()
                .ReverseMap();

            CreateMap<PurchaseItemDto, PurchaseItem>()
                .ReverseMap();

            CreateMap<CreatePurchaseItemDto, PurchaseItem>() 
                .ReverseMap();

            //CreateMap<PurchaseItemDto, StorageItem>()
            //    .ReverseMap();
        }
    }
}
