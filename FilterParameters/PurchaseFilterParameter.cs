namespace FurnitureERP.FilterParameters
{
    public class PurchaseFilterParameter
    {
        public string? Keyword { get; set; }
        public string? SupplierName { get; set; }
        public string? ItemName { get; set; }
        public string? ItemNo { get; set; }
        public DateTime? StartCreateTime { get; set; }
        public DateTime? EndCreateTime { get; set; }
        public DateTime? StartPurchaseOrderDate { get; set; }
        public DateTime? EndPurchaseOrderDate { get; set; }
        public DateTime? StartDeliveryDate { get; set; }
        public DateTime? EndDeliveryDate { get; set; }
        public int? PageNo { get; set; }
        public int? PageSize { get; set; }
    }
}
