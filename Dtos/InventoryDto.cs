namespace FurnitureERP.Dtos
{
    public class InventoryDto
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string ItemName { get; set; }
        public string ItemNo { get; set; }
        public string StdItemNo { get; set; }
        public int Qty { get; set; }
        public decimal CostPrice { get; set; }
        public string? WareName { get; set; }
        public string? PurchNo { get; set; }
        public string? SuppName { get; set; }
        public DateTime? StorageTime { get; set; }
        public string? Remark { get; set; }
        public DateTime CreateTime { get; set; }
        public string? Creator { get; set; }
        public string? StorageType { get; set; }
        public string? Tid { get; set; }
    }
}
