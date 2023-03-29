

using FurnitureERP.Models;

namespace FurnitureERP.Controllers
{
    public class UserController
    {
        [Authorize]
        public static async Task<IResult> Create(AppDbContext db, CreateUserDto userDto, HttpRequest request,IMapper mapper)
        {
            if (await db.Users.FirstOrDefaultAsync(x => x.UserName == userDto.UserName) != null)
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
        public static async Task<IResult> Get(AppDbContext db,IMapper mapper)
        {
            var ets = await db.Users.ToListAsync();
            return Results.Ok(mapper.Map<List<UserDto>>(ets));
        }

        [Authorize]
        public static async Task<IResult> Single(AppDbContext db, int id, IMapper mapper)
        {
            var et = await db.Users.SingleOrDefaultAsync(x => x.Id == id);
            return et == null ? Results.NotFound() : Results.Ok(mapper.Map<UserDto>(et));
        }

        [Authorize]
        public static async Task<IResult> Edit(AppDbContext db, int id, CreateUserDto userDto, HttpRequest request, IMapper mapper)
        {
            var et = await db.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (et == null)
            {
                return Results.BadRequest();
            }
            if (await db.Users.FirstOrDefaultAsync(x => x.Id != id && x.UserName == userDto.UserName) != null)
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
        public static async Task<IResult> Delete(AppDbContext db, int id, HttpRequest request)
        {
            var et = await db.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (et == null)
            {
                return Results.BadRequest();
            }

            db.Users.Remove(et);
            await db.SaveChangesAsync();
            return Results.NoContent();
        }
    }
}
