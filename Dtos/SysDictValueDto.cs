namespace FurnitureERP.Dtos
{
    public class SysDictValueDto
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string? DictCode { get; set; }
        public string? DataValue { get; set; }
        public string? Remark { get; set; }
        public DateTime CreateTime { get; set; }
        public string? Creator { get; set; }
        public bool? IsUsing { get; set; }
    }
}
