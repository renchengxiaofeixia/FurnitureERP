namespace FurnitureERP.Dtos
{
    public class InventoryItemMoveDto
    {
        public Guid Guid { get; set; }
        public string? MoveNo { get; set; }
        public required string ItemName { get; set; }
        public required string ItemNo { get; set; } 
        public int Quantity { get; set; }
        public decimal CostPrice { get; set; }
        public required string WareName { get; set; }
        public required string ToWareName { get; set; }
        public string? PurchaseNo { get; set; }
        public string StorageNo { get; set; }
        public string? SuppName { get; set; }
        public int MoveQuantity { get; set; }
        public string? Remark { get; set; }
    }
}
