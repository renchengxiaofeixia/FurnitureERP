

namespace FurnitureERP.Dtos
{
    public class CreateUserDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string? Remark { get; set; }
        public bool IsUsing { get; set; }
    }
}
