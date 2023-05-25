using FurnitureERP.Enums;
namespace FurnitureERP.Dtos
{
    public class ItemDeleteDto
    {
        public long Id { get; set; }
        public Guid Guid { get; set; }
        public string? ItemName { get; set; }
        public string? ItemNo { get; set; }
        public decimal Volume { get; set; }
        public decimal CostPrice { get; set; }
        public int PackageQty { get; set; }
        public string? SuppName { get; set; }
        public int PurchaseDays { get; set; }
        public bool IsCom { get; set; }
        public string? Remark { get; set; }
        public DateTime CreateTime { get; set; }
        public string? Creator { get; set; }
        public bool? IsUsing { get; set; }
        public string? PicPath { get; set; } = string.Empty;
        public int SafeQty { get; set; }
        public decimal Price { get; set; }
        public string Style { get; set; } = string.Empty;
        public string Class { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Space { get; set; } = string.Empty;

        public string Cate { get; set; } = string.Empty;
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ItemTypeEnum ItemType { get; set; }

        public List<CustomPropertyDto> CustomProperties { get; set; }
        public List<PackageDto> ItemPackages { get; set; }
    }


    public class CreateItemDeleteDto
    {
        public string ItemName { get; set; }
        public string? ItemNo { get; set; }
        public decimal Volume { get; set; }
        public decimal CostPrice { get; set; }
        public string? SuppName { get; set; }
        public int PackageQty { get; set; }
        public bool IsCom { get; set; }
        public string? Remark { get; set; }
        public bool? IsUsing { get; set; }
        public string? PicPath { get; set; }
        public int SafeQty { get; set; }
        public decimal Price { get; set; }
        public string Style { get; set; } = string.Empty;
        public string Class { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Space { get; set; } = string.Empty;
        public string Cate { get; set; } = string.Empty;
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ItemTypeEnum ItemType { get; set; }
        public IList<CreateSubItemDto> SubItems { get; set; }
    }

}
