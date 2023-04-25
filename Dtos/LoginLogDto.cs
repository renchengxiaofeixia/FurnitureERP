namespace FurnitureERP.Dtos
{
    public class LoginLogDto
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string? Ip { get; set; }
        public string? Browser { get; set; }
        public string? Os { get; set; }
        public string? Device { get; set; }
        public string? BrowserInfo { get; set; }
        public bool? Status { get; set; }
        public string? Msg { get; set; }
        public DateTime CreateTime { get; set; }
        public string? Creator { get; set; }
        public Guid MerchantGuid { get; set; }
    }

    public class CreateLoginLogDto
    {
        public string? Ip { get; set; }
        public string? Browser { get; set; }
        public string? Os { get; set; }
        public string? Device { get; set; }
        public string? BrowserInfo { get; set; }
        public bool? Status { get; set; }
        public string? Msg { get; set; }
    }
}
