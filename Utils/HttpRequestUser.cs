
namespace FurnitureERP.Utils
{
    public static class HttpRequestUser
    {
        public static User GetCurrentUser(this HttpRequest request)
        {
            var userName = request.HttpContext.User.Identity?.Name;
            var merchantGuid = request.HttpContext.User.Claims.First(c=> c.Type == ClaimTypes.GroupSid).Value;
            var merchantName = request.HttpContext.User.Claims.First(c => c.Type == ClaimTypes.GivenName).Value;

            return new User { UserName = userName, MerchantGuid = Guid.Parse(merchantGuid), MerchantName = merchantName };
        }
    }

    
}
