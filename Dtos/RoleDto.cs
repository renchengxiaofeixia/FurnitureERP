

namespace FurnitureERP.Dtos
{
    public class RoleDto
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string RoleName { get; set; }
        public string Remark { get; set; }
        public DateTime CreateTime { get; set; }
        public string? Creator { get; set; }
        public bool? IsUsing { get; set; }
    }
}
