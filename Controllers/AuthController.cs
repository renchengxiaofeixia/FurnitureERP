﻿
using FurnitureERP.Models;

namespace FurnitureERP.Controllers
{
    public class AuthController
    {
        [AllowAnonymous]
        public async static Task<IResult> Signin(AppDbContext db, PermissionRequirement permitReq, LoginDto user)
        {
            var isSubLogin = user.UserName.Contains(":");
            if (isSubLogin)
            {
                var merchantName = user.UserName.Substring(0, user.UserName.LastIndexOf(":"));
                var userName = user.UserName.Substring(user.UserName.LastIndexOf(":") +1);
                var et = await db.Users.FirstOrDefaultAsync(k => (k.IsUsing.HasValue && k.IsUsing.Value) && k.UserName == userName && k.MerchantName == merchantName && k.Password == user.Password);
                if (et == null)
                {
                    return Results.BadRequest("用户名或密码错误");
                }
                var roleIds = await (from u in db.Users
                                     join ur in db.UserRoles on u.Id equals ur.UserId
                                     select ur.RoleId).ToListAsync();
                var claims = roleIds.Select(r => new Claim(ClaimTypes.Role, r.ToString())).ToList();
                claims.Add(new Claim(ClaimTypes.GroupSid, et.MerchantGuid.ToString()));
                claims.Add(new Claim(ClaimTypes.GivenName, et.MerchantName));
                var rolePermits = await db.RolePermits.Where(p => roleIds.Any(rid => rid == p.RoleId)).Select(k => k.PermitData).ToListAsync();
                var token = JwtToken.Build(claims.ToArray(), permitReq, et);
                return Results.Ok(new { et.Id, UserName = userName, et.MerchantName, et.MerchantGuid, Token = token, Permit = rolePermits, IsAdministrator = false});
            }
            else
            {
                var merchant = await db.Merchants.FirstOrDefaultAsync(k => k.MerchantName == user.UserName && k.Password == user.Password);
                if (merchant == null)
                {
                    return Results.BadRequest("用户名或密码错误");
                }

                var claims = new[] { 
                    new Claim(ClaimTypes.Role, "0")
                    , new Claim(ClaimTypes.GroupSid, merchant.Guid.ToString())
                    , new Claim(ClaimTypes.GivenName, merchant.MerchantName)};
                var token = JwtToken.Build(claims, permitReq, new User { Id = 0,UserName = merchant.MerchantName });
                return Results.Ok(new { Id = 0, user.UserName , merchant.MerchantName, MerchantGuid = merchant.Guid, Token = token, IsAdministrator = true });
            }
        }

        [AllowAnonymous]
        public async static Task<IResult> SigninForSmsCode(AppDbContext db, PermissionRequirement permitReq, SmsLoginDto smsLoginDto)
        {
            var phoneCode = db.PhoneCodes.LastOrDefault(k => k.MobileNo == smsLoginDto.MobileNo && k.SmsCode == smsLoginDto.SmsCode && smsLoginDto.SmsCodeId == k.Id);
            if (phoneCode == null)
            {
                return Results.BadRequest("验证码错误");
            }
            if ((DateTime.Now - phoneCode.CreateTime).TotalMinutes > 5)
            {
                return Results.BadRequest("验证码已过期");
            }
            var merchant = await db.Merchants.FirstOrDefaultAsync(k => k.MobileNo == smsLoginDto.MobileNo);
            if (merchant == null)
            {
                return Results.BadRequest("用户不存在");
            }

            var claims = new[] {
                new Claim(ClaimTypes.Role, "0")
                , new Claim(ClaimTypes.GroupSid, merchant.Guid.ToString())
                , new Claim(ClaimTypes.GivenName, merchant.MerchantName)};
            var token = JwtToken.Build(claims, permitReq, new User { Id = 0, UserName = merchant.MerchantName });
            return Results.Ok(new { Id = 0, UserName = merchant.MerchantName, merchant.MerchantName, MerchantGuid = merchant.Guid, Token = token, IsAdministrator = true }); 
        }

        [AllowAnonymous]
        public static async Task<IResult> Signup(AppDbContext db, RegisterDto user)
        {
            if (user.Password != user.RePassword)
            {
                return Results.BadRequest("两次输入的密码不匹配");
            }
            var phoneCode = db.PhoneCodes.LastOrDefault(k=>k.MobileNo == user.MobileNo && k.SmsCode == user.SmsCode && user.SmsCodeId == k.Id);
            if (phoneCode == null)
            {
                return Results.BadRequest("验证码错误");
            }
            if((DateTime.Now - phoneCode.CreateTime).TotalMinutes > 5)
            {
                return Results.BadRequest("验证码已过期");
            }

            var et = await db.Merchants.FirstOrDefaultAsync(k => k.MerchantName == user.UserName || k.MobileNo == user.MobileNo);
            if (et != null)
            {
                return Results.BadRequest("用户已经存在");
            }
            var u = new Merchant();
            u.MerchantName = user.UserName;
            u.Password = user.Password;
            db.Merchants.Add(u);
            await db.SaveChangesAsync();
            return Results.Ok(user);
        }

        [AllowAnonymous]
        public static async Task<IResult> SendSmsCode(AppDbContext db, string mobileNo)
        {
            var sc = new PhoneCode();
            sc.MobileNo = mobileNo;
            sc.SmsCode = Random.Shared.Next(1000, 9999).ToString();
            db.PhoneCodes.Add(sc);
            await db.SaveChangesAsync();
            return Results.Ok(new { sc.Id, sc.SmsCode });
        }
    }
}
