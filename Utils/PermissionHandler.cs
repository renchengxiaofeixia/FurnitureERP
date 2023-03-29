
namespace FurnitureERP.Utils
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        /// <summary>
        /// 验证方案提供对象
        /// </summary>
        public IAuthenticationSchemeProvider _schemes { get; set; }
        public AppDbContext _db { get; set; }

        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="schemes"></param>
        /// <param name="rolePermitRepo"></param>
        public PermissionHandler(IAuthenticationSchemeProvider schemes, AppDbContext db)
        {
            _schemes = schemes;
            _db = db;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement pr)
        {
            var filterContext = context.Resource as DefaultHttpContext;
            var httpContext = filterContext?.HttpContext;

            // 获取系统中所有的角色和菜单的关系集合
            //if (!pr.Permits.Any())
            //{
            //    var permits = await _db.RolePermits.ToListAsync();
            //    pr.Permits = permits.Select(p => new PermissionItem
            //    {
            //        Url = p.ActionUrl,
            //        RoleId = p.RoleId.ToString(),
            //    }).ToList();
            //}

            var requestUrl = httpContext.Request.Path.Value?.ToLower();
            httpContext.Features.Set<IAuthenticationFeature>(new AuthenticationFeature
            {
                OriginalPath = httpContext.Request.Path,
                OriginalPathBase = httpContext.Request.PathBase
            });

            //判断请求是否拥有凭据，即有没有登录
            var defaultAuthenticate = await _schemes.GetDefaultAuthenticateSchemeAsync();
            if (defaultAuthenticate != null)
            {
                var authResult = await httpContext.AuthenticateAsync(defaultAuthenticate.Name);
                if (authResult?.Principal != null)
                {
                    httpContext.User = authResult.Principal;
                    // 获取当前用户的角色信息
                    //var roleIds = httpContext.User.Claims.Where(k => k.Type == pr.ClaimType).Select(k => k.Value).ToList();
                    //if (roleIds.Any(id => id == "007")) context.Succeed(pr);
                    //var mchPermit = pr.Permits.Any(p => roleIds.Contains(p.RoleId) && requestUrl == p.Url);
                    //var expirationTime = DateTime.Parse(httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Expiration)?.Value);
                    //if (roleIds.Count > 0 && expirationTime >= DateTime.Now && mchPermit)
                    context.Succeed(pr);
                }
            }
        }
    }


    public class PermissionRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// 用户权限集合，一个订单包含了很多详情，
        /// 同理，一个网站的认证发行中，也有很多权限详情(这里是Role和URL的关系)
        /// </summary>
        public List<PermissionItem> Permits { get; set; }
        /// <summary>
        /// 认证授权类型
        /// </summary>
        public string ClaimType { internal get; set; }
        /// <summary>
        /// 发行人
        /// </summary>
        public string Issuer { get; set; }
        /// <summary>
        /// 订阅人
        /// </summary>
        public string Audience { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public TimeSpan Expiration { get; set; }
        /// <summary>
        /// 签名验证
        /// </summary>
        public SigningCredentials SigningCredentials { get; set; }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="permissions">权限集合</param>
        /// <param name="claimType">声明类型</param>
        /// <param name="issuer">发行人</param>
        /// <param name="audience">订阅人</param>
        /// <param name="signingCredentials">签名验证实体</param>
        /// <param name="expiration">过期时间</param>
        public PermissionRequirement(List<PermissionItem> permits, string claimType, string issuer, string audience, SigningCredentials signingCredentials, TimeSpan expiration)
        {
            ClaimType = claimType;
            Permits = permits;
            Issuer = issuer;
            Audience = audience;
            Expiration = expiration;
            SigningCredentials = signingCredentials;
        }
    }

    public class PermissionItem
    {
        public string RoleId { get; set; }
        public string Url { get; set; }
    }
}

