namespace FurnitureERP.FilterParameters
{
    public class ItemFilterParameter
    {
        public string? Keyword { get; set; }
        public DateTime? StartCreateTime { get; set; }
        public DateTime? EndCreateTime { get; set; }
        public bool? IsCom { get; set; }
        public string? Cate { get; set; }
        public string? Status { get; set; }
        public int? PageNo { get; set; }
        public int? PageSize { get; set; }
    }
}
