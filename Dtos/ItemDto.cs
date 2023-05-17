
namespace FurnitureERP.Dtos
{
    public class ItemDto
    {
        public long Id { get; set; }
        public Guid Guid { get; set; }
        public string? ItemName { get; set; }
        public string? ItemNo { get; set; }
        public decimal Volume { get; set; }
        public decimal CostPrice { get; set; }
        public decimal PackageQty { get; set; }
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
        public string Style { get; set; }
        public string Class { get; set; }
        public string Brand { get; set; }
        public string Space { get; set; }

        public List<CustomPropertyDto> CustomProperties { get; set; }
        public List<PackageDto> ItemPackages { get; set; }
    }


    public class CreateItemDto
    {
        public string ItemName { get; set; }
        public string? ItemNo { get; set; }
        public decimal Volume { get; set; }
        public decimal CostPrice { get; set; }
        public string? SuppName { get; set; }
        public decimal PackageQty { get; set; }
        public bool IsCom { get; set; }
        public string? Remark { get; set; }
        public bool? IsUsing { get; set; }
        public string? PicPath { get; set; }
        public int SafeQty { get; set; }
        public decimal Price { get; set; }
        public string Style { get; set; }
        public string Class { get; set; }
        public string Brand { get; set; }
        public string Space { get; set; }
        public IList<CreateSubItemDto> SubItems { get; set; }
    }

    public class PackageDto
    {
        public long Id { get; set; }
        public Guid Guid { get; set; }
        public required string PackageName { get; set; }
        public required string PackageNo { get; set; }
        public string? LengthWidthHeight { get; set; }
        public decimal Volume { get; set; }
        public decimal CostPrice { get; set; }
        public int PurchaseDays { get; set; }
        public string? Remark { get; set; }
        public DateTime CreateTime { get; set; }
        public string? Creator { get; set; }
        public bool? IsUsing { get; set; }
        public int SafeQty { get; set; }
        public Guid MerchantGuid { get; set; }
    }

    public class CreatePackageDto
    {
        public required string PackageName { get; set; }
        public required string PackageNo { get; set; }
        public string? LengthWidthHeight { get; set; }
        public decimal Volume { get; set; }
        public decimal CostPrice { get; set; }
        public int PurchaseDays { get; set; }
        public string? Remark { get; set; }
        public bool? IsUsing { get; set; }
        public int SafeQty { get; set; }
    }

    public class ItemCatDto
    {
        public long Id { get; set; }
        public Guid Guid { get; set; }
        public string? CateName { get; set; }
        public string? Type { get; set; }
        public DateTime CreateTime { get; set; }
        public string? Creator { get; set; }
        public bool? IsUsing { get; set; }
    }

    public class CreateItemCatDto
    {
        public string? CateName { get; set; }
        public string? Type { get; set; }
        public bool? IsUsing { get; set; }
    }

}
