
namespace FurnitureERP.Dtos
{
    public class CreateSuppItemDto
    {
        public string? SuppName { get; set; }

        public string? ItemNo { get; set; }

        public decimal CostPrice { get; set; }

        public DateTime CreateTime { get; set; }

        public string? Creator { get; set; }
    }
}
