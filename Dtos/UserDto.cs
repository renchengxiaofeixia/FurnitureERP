

namespace FurnitureERP.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string UserName { get; set; }
        public string? Password { get; set; }
        public string? Remark { get; set; }
        public DateTime CreateTime { get; set; }
        public string? Creator { get; set; }
        public bool? IsUsing { get; set; }
    }
}
