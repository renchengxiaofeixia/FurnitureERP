namespace FurnitureERP.Dtos
{
    public class UserRoleDto
    {
        public long RoleId { get; set; }
        public List<long> UserIds { get; set; }
    }
}
