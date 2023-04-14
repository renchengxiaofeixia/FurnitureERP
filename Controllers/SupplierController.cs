

using Azure.Core;
using Microsoft.Data.SqlClient;

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

            if (SuppDto.SuppItems != null && SuppDto.SuppItems.Count > 0) {

                var suppItems = mapper.Map<List<SuppItem>>(SuppDto.SuppItems);

                suppItems.ForEach(si =>
                {
                    si.MerchantGuid = request.GetCurrentUser().MerchantGuid;
                    si.Creator = request.GetCurrentUser().UserName;
                });
                await db.SuppItems.AddRangeAsync(suppItems);
            }
            await db.Supps.AddAsync(supp);
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
        public static async Task<IResult> GetSuppItems(AppDbContext db, IMapper mapper, int id, HttpRequest request)
        {
            if (!await db.Supps.AnyAsync(x => x.Id == id && x.MerchantGuid == request.GetCurrentUser().MerchantGuid))
            {
                return Results.BadRequest("无效的数据");
            }
            var ets = from su in db.Supps
                      from si in db.SuppItems
                      where su.SuppName == si.SuppName && su.Id == id && si.MerchantGuid == request.GetCurrentUser().MerchantGuid
                      select su;
            return Results.Ok(mapper.Map<List<ItemDto>>(await ets.ToListAsync()));
        }

        [Authorize]
        public static async Task<IResult> Import(AppDbContext db, HttpRequest request, bool isErase = false)
        {
            if (!request.HasFormContentType)
                return Results.BadRequest();

            var form = await request.ReadFormAsync();
            var fi = form.Files["fi"];
            if (fi is null || fi.Length == 0)
                return Results.BadRequest();

            var svrFn = $"{DateTime.Now.Ticks}{Path.GetExtension(fi.FileName)}";
            var svrpath = Path.Combine(AppContext.BaseDirectory, "excel", svrFn);
            await using var stream = fi.OpenReadStream();
            using var fs = File.Create(svrpath);
            await stream.CopyToAsync(fs);

            var fieldsMapper = new Dictionary<string, string>()
            {
                { "供应商名称","SuppName"},
                { "商品编码","ItemNo" },
                { "采购价","CostPrice"}
            };
            var (rt, suppitems) = Util.ReadExcel<SuppItemImp>(fs, fieldsMapper);
            if (rt)
            {
                suppitems.ForEach(it =>
                {
                    it.Guid = Guid.NewGuid();
                    it.Creator = request.GetCurrentUser().UserName;
                    it.MerchantGuid = request.GetCurrentUser().MerchantGuid;
                    it.CreateTime = DateTime.Now;
                });
                await db.SuppItemImps.AddRangeAsync(suppitems);
                await db.SaveChangesAsync();

                //获取cell
                //var cellValues = db.Database.SqlQueryRaw<string>(GetCellSql, new SqlParameter("@MerchantGuid", request.GetCurrentUser().MerchantGuid)).ToList();

                var itemNos =  db.Items.Where(k => k.MerchantGuid == request.GetCurrentUser().MerchantGuid).Select(k => k.ItemNo).ToList();

                var cellValues = db.SuppItemImps.Where(k => !itemNos.Any(i => i == k.ItemNo && k.MerchantGuid == request.GetCurrentUser().MerchantGuid)).Select(k=>k.ItemNo).ToList();

                if (cellValues.Count > 0)
                {
                    var execlPath = Util.MarkerCell(fs, fieldsMapper.Count, cellValues);
                    return Results.BadRequest(new
                    {
                        url = execlPath,
                        msg = "导入表格中存在商品表中不存在的商品，已对不存在或错误商品编码标红处理，请修改或者添加好商品再进行信息导入！"
                    });
                }
                else
                {
                    await db.Database.ExecuteSqlRawAsync("p_syncimpsuppitem @MerchantGuid"
                      , new SqlParameter("@MerchantGuid", request.GetCurrentUser().MerchantGuid));
                }
                
            }
            return Results.Ok(new { isOk = true });
        }


        [Authorize]
        public static async Task<IResult> Single(AppDbContext db, int id, IMapper mapper, HttpRequest request)
        {
            var et = await db.Supps.SingleOrDefaultAsync(x => x.Id == id && x.MerchantGuid == request.GetCurrentUser().MerchantGuid);
            return et == null ? Results.NotFound() : Results.Ok(mapper.Map<SuppDto>(et));
        }

        [Authorize]
        public static async Task<IResult> Edit(AppDbContext db, int id, CreateSuppDto SuppDto, HttpRequest request, IMapper mapper)
        {
            var UserName = request.GetCurrentUser().UserName;
            var MerchantGuid = request.GetCurrentUser().MerchantGuid;

            var et = await db.Supps.FirstOrDefaultAsync(x => x.Id == id && x.MerchantGuid == MerchantGuid);
            if (et == null)
            {
                return Results.BadRequest();
            }
            if (await db.Supps.FirstOrDefaultAsync(x => x.Id != id && x.MerchantGuid== MerchantGuid && x.SuppName == SuppDto.SuppName) != null)
            {
                return Results.BadRequest("存在相同的供应商名称");
            }
            if (SuppDto.SuppItems != null && SuppDto.SuppItems.Count > 0) { 
                //删除下级供应商产品
                var  removeItems = await db.SuppItems.Where(k => k.SuppName == et.SuppName && k.MerchantGuid == MerchantGuid).ToListAsync();
                db.SuppItems.RemoveRange(removeItems);

                var suppItems = mapper.Map<List<SuppItem>>(SuppDto.SuppItems);

                suppItems.ForEach(si =>
                {
                    si.MerchantGuid = MerchantGuid;
                    si.Creator = request.GetCurrentUser().UserName;
                });

                await db.SuppItems.AddRangeAsync(suppItems);
            }
            et.SuppName = SuppDto.SuppName;
            et.SuppCompany = SuppDto.SuppCompany;
            et.SuppMobile = SuppDto.SuppMobile;
            et.Remark = SuppDto.Remark;
            et.Creator = UserName;
            et.MerchantGuid = MerchantGuid;
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
