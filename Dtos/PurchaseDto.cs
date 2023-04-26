namespace FurnitureERP.Dtos
{
    public class PurchaseDto
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string PurchaseNo { get; set; }
        public string SuppName { get; set; }
        public string WareName { get; set; }
        public decimal AggregateAmount { get; set; }
        public string SettlementMode { get; set; }
        public bool IsAudit { get; set; }
        public string? AuditUser { get; set; }
        public DateTime? AuditDate { get; set; }
        public bool IsPrint { get; set; }
        public string? PrintUser { get; set; }
        public DateTime? PrintDate { get; set; }
        public string? OuterNo { get; set; }
        public DateTime PurchaseOrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime CreateTime { get; set; }
        public string? Creator { get; set; }
        public decimal PaidFee { get; set; }
        public string? Remark { get; set; }
        public bool IsFromTrade { get; set; }
        public string? Tid { get; set; }
        public string ItemNos { get; set; }
    }

    public class CreatePurchaseDto
    {
        public string PurchaseNo { get; set; }
        public string SuppName { get; set; }
        public string WareName { get; set; }
        public string SettlementMode { get; set; }
        public string? OuterNo { get; set; }
        public DateTime PurchaseOrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public decimal PaidFee { get; set; }
        public string? Remark { get; set; }
        public bool IsFromTrade { get; set; }
        public string? Tid { get; set; }

        public List<CreatePurchaseItemDto> ItemDtos { get; set;}
    }

    public class PurchaseItemDto
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string PurchaseNo { get; set; }
        public string SuppName { get; set; } 
        public string ItemName { get; set; } 
        public string ItemNo { get; set; } 
        public string? StdItemNo { get; set; }
        public decimal CostPrice { get; set; }
        public int PurchaseNum { get; set; }
        public decimal Amount { get; set; }
        public int StorageNum { get; set; }
        public int CancelNum { get; set; }
        public string? Remark { get; set; }
        public DateTime? CreateTime { get; set; }
        public string? Creator { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public bool IsMade { get; set; }
        public Guid? OrderGuid { get; set; }
        public Guid MerchantGuid { get; set; }
        public List<PurchaseItemDto> PackageDtos { get; set; }
    }

    public class CreatePurchaseItemDto
    {
        public Guid Guid { get; set; }
        public string PurchaseNo { get; set; }
        public string SuppName { get; set; }
        public string ItemName { get; set; }
        public string ItemNo { get; set; }
        public string? StdItemNo { get; set; }
        public decimal CostPrice { get; set; }
        public int PurchaseNum { get; set; }
        public string? Remark { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public bool IsMade { get; set; }
        public Guid? OrderGuid { get; set; }
        public List<CreatePurchaseItemDto> PackageDtos { get; set; }
    }
}
