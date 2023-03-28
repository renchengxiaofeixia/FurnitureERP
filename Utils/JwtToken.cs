using FurnitureERP.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FurnitureERP.Utils
{
    public class JwtToken
    {
        public static string Build(Claim[] roleClaims, PermissionRequirement permitReq, User user)
        {
            var claims = new List<Claim> {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
                        new Claim(ClaimTypes.Expiration, DateTime.Now.AddSeconds(permitReq.Expiration.TotalSeconds).ToString()) };
            claims.AddRange(roleClaims);
            var tokenDescriptor = new JwtSecurityToken(permitReq.Issuer, permitReq.Audience, claims,
                expires: DateTime.Now.Add(permitReq.Expiration), signingCredentials: permitReq.SigningCredentials);
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
