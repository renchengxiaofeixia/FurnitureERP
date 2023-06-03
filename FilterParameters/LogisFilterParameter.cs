namespace FurnitureERP.FilterParameters
{
    public class LogisFilterParameter
    {
        public string? Keyword { get; set; }
        public DateTime? StartCreateTime { get; set; }
        public DateTime? EndCreateTime { get; set; }
        public int? PageNo { get; set; }
        public int? PageSize { get; set; }
    }
}
