
namespace FurnitureERP.Dtos
{
    public class PropertyConfigDto
    {
        public long Id { get; set; }
        public Guid Guid { get; set; }
        public string ModuleName { get; set; }
        public string ModuleNo { get; set; }
        //public string PropertyConfigJson { get; set; }
        public DateTime CreateTime { get; set; }
        public string? Creator { get; set; }
        public Guid MerchantGuid { get; set; }

        public List<PropertyDto> Properties { get; set; }
    }


    public class CreatePropertyConfigDto
    {
        public string ModuleName { get; set; }
        public string ModuleNo { get; set; }
        //public string PropertyConfigJson { get; set; }
        public List<PropertyDto> Properties { get; set; }
    }

    public class PropertyDto
    {
        public string? PropertyName { get; set; }
        public string? PropertyType { get; set; }
        public string? DefaultValue { get; set; }
        public List<string> PropertyValues { get; set; }
    }
}
