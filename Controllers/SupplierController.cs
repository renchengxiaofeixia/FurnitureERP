

namespace FurnitureERP.Controllers
{
    public class SupplierController
    {
        [Authorize]
        public static async Task<IResult> Create(AppDbContext db, CreateSuppDto SuppDto, HttpRequest request,IMapper mapper)
        {
            if (await db.Supps.FirstOrDefaultAsync(x => x.SuppName == SuppDto.SuppName) != null)
            {
                return Results.BadRequest("存在相同的供应商名");
            }
            var supp = mapper.Map<Supp>(SuppDto);
            supp.Creator = request.GetCurrentUser().UserName;
            supp.MerchantGuid = request.GetCurrentUser().MerchantGuid;
            db.Supps.Add(supp);
            await db.SaveChangesAsync();
            return Results.Created($"/supplier/{supp.Id}", supp);
        }

        [Authorize]
        public static async Task<IResult> Get(AppDbContext db,IMapper mapper)
        {
            var ets = await db.Users.ToListAsync();
            return Results.Ok(mapper.Map<List<SuppDto>>(ets));
        }

        [Authorize]
        public static async Task<IResult> Single(AppDbContext db, int id, IMapper mapper)
        {
            var et = await db.Supps.SingleOrDefaultAsync(x => x.Id == id);
            return et == null ? Results.NotFound() : Results.Ok(mapper.Map<SuppDto>(et));
        }

        [Authorize]
        public static async Task<IResult> Edit(AppDbContext db, int id, CreateSuppDto SuppDto, HttpRequest request, IMapper mapper)
        {
            var et = await db.Supps.FirstOrDefaultAsync(x => x.Id == id);
            if (et == null)
            {
                return Results.BadRequest();
            }
            if (await db.Supps.FirstOrDefaultAsync(x => x.Id != id && x.SuppName == SuppDto.SuppName) != null)
            {
                return Results.BadRequest("存在相同的供应商名");
            }
            et.SuppName = SuppDto.SuppName;
            et.SuppCompany = SuppDto.SuppCompany;
            et.SuppMobile = SuppDto.SuppMobile;
            et.Remark = SuppDto.Remark;
            et.Creator = request.GetCurrentUser().UserName;
            et.MerchantGuid = request.GetCurrentUser().MerchantGuid;
            await db.SaveChangesAsync();
            return Results.Ok(et);
        }

        [Authorize]
        public static async Task<IResult> Delete(AppDbContext db, int id, HttpRequest request)
        {
            var et = await db.Supps.FirstOrDefaultAsync(x => x.Id == id);
            if (et == null)
            {
                return Results.BadRequest();
            }

            db.Supps.Remove(et);
            await db.SaveChangesAsync();
            return Results.NoContent();
        }
    }
}
