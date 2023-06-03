namespace FurnitureERP.FilterParameters
{
    public class StorageFilterParameter
    {
        public string? Keyword { get; set; }
        public string? SupplierName { get; set; }
        public string? ItemName { get; set; }
        public string? ItemNo { get; set; }
        public DateTime? StartCreateTime { get; set; }
        public DateTime? EndCreateTime { get; set; }
        public DateTime? StartStorageDate { get; set; }
        public DateTime? EndStorageDate { get; set; }
        public int? PageNo { get; set; }
        public int? PageSize { get; set; }
    }
}
