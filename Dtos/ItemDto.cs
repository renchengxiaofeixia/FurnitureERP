
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
        public string? PicPath { get; set; }
        public int SafeQty { get; set; }
        public decimal Price { get; set; }

        //public IList<ItemDto> SubItems { get; set; }
    }
}
