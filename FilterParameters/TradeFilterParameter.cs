namespace FurnitureERP.FilterParameters
{
    public class TradeFilterParameter
    {
        public string? Keyword { get; set; }
        public string? ReceiverName { get; set; }
        public string? ReceiverMobile { get; set; }
        public string? LogisName { get; set; }
        public string? LogisNo { get; set; }
        public DateTime? StartCreateTime { get; set; }
        public DateTime? EndCreateTime { get; set; }
        public DateTime? StartStorageDate { get; set; }
        public DateTime? EndStorageDate { get; set; }
        public DateTime? StartPayTime { get; set; }
        public DateTime? EndPayTime { get; set; }
        public DateTime? StartPrintDate { get; set; }
        public DateTime? EndPrintDate { get; set; }
        public int? PageNo { get; set; }
        public int? PageSize { get; set; }
    }
}
