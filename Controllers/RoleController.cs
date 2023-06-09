﻿
using Azure.Core;
using Microsoft.AspNetCore.Identity;

namespace FurnitureERP.Controllers
{
    public class RoleController
    {
        [Authorize]
        public static async Task<IResult> Create(AppDbContext db, CreateRoleDto roleDto, HttpRequest request,IMapper mapper)
        {
            if (await db.Roles.FirstOrDefaultAsync(x => x.MerchantGuid == request.GetCurrentUser().MerchantGuid && x.RoleName == roleDto.RoleName) != null)
            {
                return Results.BadRequest("存在相同的角色名");
            }
            var r = mapper.Map<Role>(roleDto);
            r.Creator = request.GetCurrentUser().UserName;
            r.MerchantGuid = request.GetCurrentUser().MerchantGuid;
            db.Roles.Add(r);
            await db.SaveChangesAsync();
            return Results.Created($"/role/{r.Id}", r);
        }

        [Authorize]
        public static async Task<IResult> Get(AppDbContext db,IMapper mapper, HttpRequest request)
        {
            var ets = await db.Roles.Where(x => x.MerchantGuid == request.GetCurrentUser().MerchantGuid).ToListAsync();
            var roleIds = ets.Select(k=>k.Id).ToList();
            var users = await db.Users.Where(x => x.MerchantGuid == request.GetCurrentUser().MerchantGuid).ToListAsync();
            var roleUsers = await db.UserRoles.Where(x => roleIds.Any(j=>j == x.RoleId)).ToListAsync();
            var dtos = mapper.Map<List<RoleDto>>(ets);
            dtos.ForEach(r =>
            {
                r.UsersName = string.Join(" ", (from u in users
                                                join ru in roleUsers
                                                on u.Id equals ru.UserId
                                                where ru.RoleId == r.Id
                                                select u.UserName));
            });
            return Results.Ok(dtos);
        }

        [Authorize]
        public static async Task<IResult> Single(AppDbContext db, long id, IMapper mapper, HttpRequest request)
        {
            var et = await db.Roles.SingleOrDefaultAsync(x => x.Id == id && x.MerchantGuid == request.GetCurrentUser().MerchantGuid);
            return et == null ? Results.NotFound() : Results.Ok(mapper.Map<RoleDto>(et));
        }

        [Authorize]
        public static async Task<IResult> Edit(AppDbContext db, long id, CreateRoleDto roleDto, HttpRequest request)
        {
            var et = await db.Roles.FirstOrDefaultAsync(x => x.Id == id && x.MerchantGuid == request.GetCurrentUser().MerchantGuid);
            if (et == null)
            {
                return Results.BadRequest("无效的数据");
            }
            if (await db.Roles.FirstOrDefaultAsync(x => x.Id != id && x.MerchantGuid == request.GetCurrentUser().MerchantGuid && x.RoleName == roleDto.RoleName) != null)
            {
                return Results.BadRequest("存在相同的角色名");
            }
            et.RoleName = roleDto.RoleName;
            et.Remark = roleDto.Remark;
            await db.SaveChangesAsync();
            return Results.Ok(et);
        }

        [Authorize]
        public static async Task<IResult> Delete(AppDbContext db, long id, HttpRequest request)
        {
            var et = await db.Roles.FirstOrDefaultAsync(x => x.Id == id && x.MerchantGuid == request.GetCurrentUser().MerchantGuid);
            if (et == null)
            {
                return Results.BadRequest("无效的数据");
            }
            db.Roles.Remove(et);
            await db.SaveChangesAsync();
            return Results.NoContent();
        }

        [Authorize]
        public static async Task<IResult> CreateUserRole(AppDbContext db, UserRoleDto userRole, HttpRequest request)
        {
            var userRoles = await db.UserRoles.Where(x=>x.RoleId == userRole.RoleId).ToListAsync();
            db.UserRoles.RemoveRange(userRoles);
            
            //var users = db.Users.Where(x=>userRole.UserIds.Contains(x.Id));
            db.UserRoles.AddRange(userRole.UserIds.Select(uid=>new UserRole() {
                RoleId = userRole.RoleId,
                UserId = uid,
                CreateTime = DateTime.Now,
                Creator = request.GetCurrentUser().UserName                
            }));
            await db.SaveChangesAsync();
            var role = await db.Roles.SingleOrDefaultAsync(x => x.Id == userRole.RoleId);
            return Results.Ok(role);
        }

        [Authorize]
        public static async Task<IResult> GetUserRoles(AppDbContext db, long id, HttpRequest request) 
        {
            var et = await db.Roles.FirstOrDefaultAsync(x => x.Id == id && x.MerchantGuid == request.GetCurrentUser().MerchantGuid);
            if (et == null)
            {
                return Results.BadRequest("无效的数据");
            }
            return Results.Ok((from u in db.Users
                    join ur in db.UserRoles on u.Id equals ur.UserId
                    where ur.RoleId == id
                               select u).ToList());
        }
    }
}
