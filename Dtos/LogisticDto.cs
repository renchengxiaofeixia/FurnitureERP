namespace FurnitureERP.Dtos
{
    public class LogisticDto
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string LogisName { get; set; }
        public string? LogisMobile { get; set; }
        public string? DefProv { get; set; }
        public string? LogisAddr { get; set; }
        public bool IsDef { get; set; }
        public string? Remark { get; set; }
        public bool IsUsing { get; set; }
        public DateTime CreateTime { get; set; }
        public string? Creator { get; set; }
    }

    public class CreateLogisticDto
    {
        public string LogisName { get; set; }
        public string? LogisMobile { get; set; }
        public string? DefProv { get; set; }
        public string? LogisAddr { get; set; }
        public bool IsDef { get; set; }
        public string? Remark { get; set; }
        public bool IsUsing { get; set; }
    }

    public class LogisPointDto
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string LogisName { get; set; } 
        public string? State { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public string PointName { get; set; }
        public string? PointAdress { get; set; }
        public string? PorintMobile { get; set; }
        public decimal LowestPrice { get; set; }
        public decimal Price { get; set; }
        public decimal ZhiPrice { get; set; }
        public bool IsPost { get; set; }
        public int EstTime { get; set; }
        public string? Remark { get; set; }
        public bool IsUsing { get; set; }
        public string Creator { get; set; }
        public DateTime CreateTime { get; set; }
    }

    public class CreateLogisPointDto
    {
        public string LogisName { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public string PointName { get; set; }
        public string? PointAdress { get; set; }
        public string? PorintMobile { get; set; }
        public decimal LowestPrice { get; set; }
        public decimal Price { get; set; }
        public decimal ZhiPrice { get; set; }
        public bool IsPost { get; set; }
        public int EstTime { get; set; }
        public string? Remark { get; set; }
        public bool IsUsing { get; set; }
    }
}
