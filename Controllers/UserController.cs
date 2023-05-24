
using Azure.Core;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureERP.Controllers
{
    public class UserController
    {
        [Authorize]
        public static async Task<IResult> Create(AppDbContext db, CreateUserDto userDto, HttpRequest request,IMapper mapper)
        {
            if (await db.Users.FirstOrDefaultAsync(x => x.MerchantGuid == request.GetCurrentUser().MerchantGuid && x.UserName == userDto.UserName) != null)
            {
                return Results.BadRequest("存在相同的用户名");
            }
            var user = mapper.Map<User>(userDto);
            user.Creator = request.GetCurrentUser().UserName;
            user.MerchantGuid = request.GetCurrentUser().MerchantGuid;
            user.MerchantName = request.GetCurrentUser().MerchantName;
            db.Users.Add(user);
            await db.SaveChangesAsync();
            return Results.Created($"/user/{user.Id}", user);
        }

        [Authorize]
        public static async Task<IResult> Get(AppDbContext db,IMapper mapper, HttpRequest request)
        {
            var ets = await db.Users.Where(x => x.MerchantGuid == request.GetCurrentUser().MerchantGuid).ToListAsync();
            return Results.Ok(mapper.Map<List<UserDto>>(ets));
        }

        [Authorize]
        public static async Task<IResult> Page(AppDbContext db, IMapper mapper
            , string? keyword
            , int pageNo, int pageSize)
        {
      
            IQueryable<User> users = db.Users;          
            if (!string.IsNullOrEmpty(keyword))
            {
                users = users.Where(k => k.UserName.Contains(keyword));
            }
           
            var page = await Pagination<User>.CreateAsync(users, pageNo, pageSize);
            page.Items = mapper.Map<List<UserDto>>(page.Items);
            return Results.Ok(page);
        }

        [Authorize]
        public static async Task<IResult> Single(AppDbContext db, long id, IMapper mapper, HttpRequest request)
        {
            var et = await db.Users.SingleOrDefaultAsync(x => x.Id == id && x.MerchantGuid == request.GetCurrentUser().MerchantGuid);
            return et == null ? Results.NotFound() : Results.Ok(mapper.Map<UserDto>(et));
        }

        [Authorize]
        public static async Task<IResult> Edit(AppDbContext db, long id, CreateUserDto userDto, HttpRequest request, IMapper mapper)
        {
            var et = await db.Users.FirstOrDefaultAsync(x => x.Id == id && x.MerchantGuid == request.GetCurrentUser().MerchantGuid);
            if (et == null)
            {
                return Results.BadRequest();
            }
            if (await db.Users.FirstOrDefaultAsync(x => x.Id != id && x.MerchantGuid== request.GetCurrentUser().MerchantGuid && x.UserName == userDto.UserName) != null)
            {
                return Results.BadRequest("存在相同的用户名");
            }
            et.UserName = userDto.UserName;
            et.Password = userDto.Password;
            et.Remark = userDto.Remark;
            et.MerchantGuid = request.GetCurrentUser().MerchantGuid;
            et.MerchantName = request.GetCurrentUser().MerchantName;
            await db.SaveChangesAsync();
            return Results.Ok(et);
        }

        [Authorize]
        public static async Task<IResult> CreateUserRole(AppDbContext db, RoleUserDto userRole, HttpRequest request)
        {
            await db.UserRoles.Where(x => x.UserId == userRole.UserId).ExecuteDeleteAsync();

            //var roles = db.Roles.Where(x => userRole.RoleIds.Contains(x.Id));
            db.UserRoles.AddRange(userRole.RoleIds.Select(rid => new UserRole()
            {
                RoleId = rid,
                UserId = userRole.UserId,
                CreateTime = DateTime.Now,
                Creator = request.GetCurrentUser().UserName
            }));
            await db.SaveChangesAsync();
            var user = await db.Users.SingleOrDefaultAsync(x => x.Id == userRole.UserId);
            return Results.Ok(user);
        }


        [Authorize]
        public static async Task<IResult> Delete(AppDbContext db, long id, HttpRequest request)
        {
            var et = await db.Users.FirstOrDefaultAsync(x => x.Id == id && x.MerchantGuid == request.GetCurrentUser().MerchantGuid);
            if (et == null)
            {
                return Results.BadRequest();
            }
            db.RecordDeletes.Add(new RecordDelete { 
                RecordId = et.Id,
                CreateTime = DateTime.Now,
                Creator = request.GetCurrentUser().UserName,
                MerchantGuid= request.GetCurrentUser().MerchantGuid,
                JsonTypeName = et.GetType().Name,
                JsonRecord = JsonSerializer.Serialize(et)
            });
            db.Users.Remove(et);
            await db.SaveChangesAsync();
            return Results.NoContent();
        }
    }
}
