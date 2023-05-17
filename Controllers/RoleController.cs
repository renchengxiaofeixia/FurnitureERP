
using Azure.Core;

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
            return Results.Ok(mapper.Map<List<RoleDto>>(ets));
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
            await db.SaveChangesAsync();
            return Results.NoContent();
        }

        [Authorize]
        public static async Task<IResult> CreateUserRole(AppDbContext db, UserRoleDto userRole, HttpRequest request)
        {
            await db.UserRoles.Where(x=>x.RoleId == userRole.RoleId).ExecuteDeleteAsync();
            
            var users = db.Users.Where(x=>userRole.UserIds.Contains(x.Id));
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
        public static async Task<IResult> GetUserRoles(AppDbContext db, long roleId, HttpRequest request) 
        {
            var et = await db.Roles.FirstOrDefaultAsync(x => x.Id == roleId && x.MerchantGuid == request.GetCurrentUser().MerchantGuid);
            if (et == null)
            {
                return Results.BadRequest("无效的数据");
            }
            return Results.Ok((from u in db.Users
                    join ur in db.UserRoles on u.Id equals ur.UserId
                    where ur.RoleId == roleId
                    select u).ToList());
        }
    }
}
