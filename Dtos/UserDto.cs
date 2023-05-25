

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
        public string HeadPic { get; set; } = string.Empty;
        public string MobileNo { get; set; } = string.Empty;

    }


    public class CreateUserDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string? Remark { get; set; }
        public bool IsUsing { get; set; }
        public string HeadPic { get; set; } = string.Empty;
        public string MobileNo { get; set; } = string.Empty;
    }
}
