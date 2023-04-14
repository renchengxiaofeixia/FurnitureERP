namespace FurnitureERP.Dtos
{
    public class InventoryItemAdjustDto
    {
        public Guid Guid { get; set; }
        public string? AdjustNo { get; set; }
        public required string ItemName { get; set; } 
        public required string ItemNo { get; set; }
        public int Quantity { get; set; }
        public decimal CostPrice { get; set; }
        public string? WareName { get; set; }
        public string? PurchaseNo { get; set; }
        public string StorageNo { get; set; }
        public string? SuppName { get; set; }
        public int AdjustQuantity { get; set; }
        public string? Remark { get; set; }
    }
}
