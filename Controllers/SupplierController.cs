

namespace FurnitureERP.Controllers
{
    public class SupplierController
    {
        [Authorize]
        public static async Task<IResult> Create(AppDbContext db, CreateSuppDto SuppDto, HttpRequest request,IMapper mapper)
        {
            if (await db.Supps.FirstOrDefaultAsync(x => x.MerchantGuid == request.GetCurrentUser().MerchantGuid && x.SuppName == SuppDto.SuppName) != null)
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
        public static async Task<IResult> Get(AppDbContext db,IMapper mapper, HttpRequest request)
        {
            var ets = await db.Supps.Where(x => x.MerchantGuid == request.GetCurrentUser().MerchantGuid).ToListAsync();
            return Results.Ok(mapper.Map<List<SuppDto>>(ets));
        }

        [Authorize]
        public static async Task<IResult> GetSuppItems(AppDbContext db, IMapper mapper, int id)
        {
            if (!await db.Supps.AnyAsync(x => x.Id == id))
            {
                return Results.BadRequest("无效的数据");
            }
            var ets = from su in db.Supps
                      from si in db.SuppItems
                      where su.SuppName == si.ItemNo && su.Id == id
                      select su;
            return Results.Ok(mapper.Map<List<ItemDto>>(await ets.ToListAsync()));
        }

        [Authorize]
        public static async Task<IResult> Import(AppDbContext db, HttpRequest request)
        {
            if (!request.HasFormContentType)
                return Results.BadRequest();

            var form = await request.ReadFormAsync();
            var fi = form.Files["fi"];
            if (fi is null || fi.Length == 0)
                return Results.BadRequest();
            if (!Path.GetExtension(fi.FileName).EndsWith("xlsx"))
                return Results.BadRequest("文件格式错误");
            var svrFn = $"{DateTime.Now.Ticks}{Path.GetExtension(fi.FileName)}";
            var svrpath = Path.Combine(AppContext.BaseDirectory, "excel", svrFn);
            await using var stream = fi.OpenReadStream();
            using var fs = File.Create(svrpath);
            await stream.CopyToAsync(fs);


            return Results.Ok();
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
            var et = await db.Supps.FirstOrDefaultAsync(x => x.Id == id && x.MerchantGuid == request.GetCurrentUser().MerchantGuid);
            if (et == null)
            {
                return Results.BadRequest();
            }
            if (await db.Supps.FirstOrDefaultAsync(x => x.Id != id && x.MerchantGuid== request.GetCurrentUser().MerchantGuid && x.SuppName == SuppDto.SuppName) != null)
            {
                return Results.BadRequest("存在相同的供应商名称");
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
            var et = await db.Supps.FirstOrDefaultAsync(x => x.Id == id && x.MerchantGuid == request.GetCurrentUser().MerchantGuid);
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
