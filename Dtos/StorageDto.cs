namespace FurnitureERP.Dtos
{
    public class StorageDto
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string? StorageNo { get; set; }
        public DateTime StorageDate { get; set; }
        public string? StorageType { get; set; }
        public string? SuppName { get; set; }
        public string? WareName { get; set; }
        public string? Remark { get; set; }
        public DateTime CreateTime { get; set; }
        public string? Creator { get; set; }
        public bool IsAudit { get; set; }
        public string? AuditUser { get; set; }
        public DateTime? AuditDate { get; set; }
        public string? PurchaseNo { get; set; }
        public string ItemNos { get; set; }
    }

    public class CreateStorageDto
    {
        public string? StorageNo { get; set; }
        public DateTime StorageDate { get; set; }
        public string? StorageType { get; set; }
        public string? SuppName { get; set; }
        public string? WareName { get; set; }
        public string? Remark { get; set; }
        public string? PurchaseNo { get; set; }
    }

    public class StorageOrderDto
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string? StorageNo { get; set; }
        public string? ItemName { get; set; }
        public string? ItemNo { get; set; }
        public string? StdItemNo { get; set; }
        public decimal CostPrice { get; set; }
        public int PurchNum { get; set; }
        public int StorageNum { get; set; }
        public bool IsMade { get; set; }
        public string? Remark { get; set; }
        public DateTime CreateTime { get; set; }
        public string? Creator { get; set; }
        public string? SuppName { get; set; }
    }

    public class CreateStorageOrderDto
    {
        public string? StorageNo { get; set; }
        public string? ItemName { get; set; }
        public string? ItemNo { get; set; }
        public string? StdItemNo { get; set; }
        public decimal CostPrice { get; set; }
        public int PurchNum { get; set; }
        public int StorageNum { get; set; }
        public bool IsMade { get; set; }
        public string? Remark { get; set; }
        public string? SuppName { get; set; }
    }
}
