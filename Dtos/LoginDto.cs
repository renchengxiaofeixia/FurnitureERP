namespace FurnitureERP.Dtos
{
    public class LoginDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class SmsLoginDto
    {
        public string MobileNo { get; set; }
        public string SmsCode { get; set; }
        public long SmsCodeId { get; set; }

    }
}
