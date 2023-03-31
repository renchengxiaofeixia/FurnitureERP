namespace FurnitureERP.Dtos
{
    public class WarehouseDto
    {
        public long Id { get; set; }
        public Guid Guid { get; set; }
        public string WarehouseName { get; set; }
        public string Remark { get; set; } 
        public DateTime CreateTime { get; set; }
        public string? Creator { get; set; }
        public bool? IsUsing { get; set; }
    }

    public class CreateWarehouseDto
    {
        public string WarehouseName { get; set; }
        public string Remark { get; set; }
    }
}
