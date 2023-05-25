namespace FurnitureERP.Dtos
{
    public class UserRoleDto
    {
        public long RoleId { get; set; }
        public List<long> UserIds { get; set; }
    }
    public class RoleUserDto
    {
        public long UserId { get; set; }
        public List<long> RoleIds { get; set; }
    }
}
