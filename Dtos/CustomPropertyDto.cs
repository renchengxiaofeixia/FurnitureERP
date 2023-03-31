namespace FurnitureERP.Dtos
{
    public class CustomPropertyDto
    {
        public long Id { get; set; }
        public Guid Guid { get; set; }
        public string ModuleNo { get; set; }
        public string? PropertyName { get; set; }
        public string? PropertyValue { get; set; }
        public string? PropertyType { get; set; }
        public DateTime CreateTime { get; set; }
        public string? Creator { get; set; }
        public Guid MerchantGuid { get; set; }
    }

    public class CreateCustomPropertyDto
    {
        public string ModuleNo { get; set; }
        public string? PropertyName { get; set; }
        public string? PropertyValue { get; set; }
        public string? PropertyType { get; set; }
    }
}
