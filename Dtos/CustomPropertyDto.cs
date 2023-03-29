
namespace FurnitureERP.Dtos
{
    public class CustomPropertyDto
    {
        public long Id { get; set; }
        public Guid Guid { get; set; }
        public string ModuleName { get; set; }
        public string ModuleNo { get; set; }
        public string PropertyConfigJson { get; set; }
        public DateTime CreateTime { get; set; }
        public string? Creator { get; set; }
        public Guid MerchantGuid { get; set; }
    }
}
