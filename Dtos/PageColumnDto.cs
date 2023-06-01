

namespace FurnitureERP.Dtos
{
    public class PageColumnDto
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string PageNamme { get; set; }
        public string ColumnJson { get; set; }
        public DateTime CreateTime { get; set; }
        public string? Creator { get; set; }
    }

    public class CreatePageColumnDto
    {
        public string PageNamme { get; set; }
        public string ColumnJson { get; set; }
    }
}
