
namespace FurnitureERP.Dtos
{
    public class CreateSubItemDto
    {
        public string? ItemNo { get; set; }

        public string? SubItemNo { get; set; }

        public int Num { get; set; }

        public string? Remark { get; set; }

        public DateTime CreateTime { get; set; }

        public string? Creator { get; set; }
    }
}
