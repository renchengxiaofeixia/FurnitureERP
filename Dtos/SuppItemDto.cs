

namespace FurnitureERP.Dtos
{
    public class SuppItemDto
    {
        public long Id { get; set; }
        public Guid Guid { get; set; }
        public string? SuppName { get; set; }
        public string? ItemNo { get; set; }
        public decimal CostPrice { get; set; }
        public DateTime CreateTime { get; set; }
        public string? Creator { get; set; }
    }


    public class CreateSuppItemDto
    {
        public string? SuppName { get; set; }
        public string? ItemNo { get; set; }
        public decimal CostPrice { get; set; }
    }
}
