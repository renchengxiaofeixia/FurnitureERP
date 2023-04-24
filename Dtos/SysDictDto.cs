namespace FurnitureERP.Dtos
{
    public class SysDictDto
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string? DictCode { get; set; }
        public string? DictName { get; set; }
        public string? Remark { get; set; }
        public DateTime CreateTime { get; set; }
        public string? Creator { get; set; }
        public required bool IsUsing { get; set; }
    }
}
