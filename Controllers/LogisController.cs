
using Azure.Core;

namespace FurnitureERP.Controllers
{
    public class LogisController
    {
        [Authorize]
        public static async Task<IResult> Create(AppDbContext db, CreateLogisticDto logisticDto, HttpRequest request,IMapper mapper)
        {
            if (await db.Logistics.FirstOrDefaultAsync(x => x.MerchantGuid == request.GetCurrentUser().MerchantGuid && x.LogisName == logisticDto.LogisName) != null)
            {
                return Results.BadRequest("存在相同的物流名称");
            }
            var r = mapper.Map<Logistic>(logisticDto);
            r.Creator = request.GetCurrentUser().UserName;
            r.MerchantGuid = request.GetCurrentUser().MerchantGuid;
            await db.Logistics.AddAsync(r);
            await db.SaveChangesAsync();
            return Results.Created($"/logis/{r.Id}", r);
        }

        [Authorize]
        public static async Task<IResult> Get(AppDbContext db, IMapper mapper, HttpRequest request)
        {
            var ets = await db.Logistics.Where(x=> x.MerchantGuid == request.GetCurrentUser().MerchantGuid).ToListAsync();
            return Results.Ok(mapper.Map<List<LogisticDto>>(ets));
        }

        [Authorize]
        public static async Task<IResult> Page(AppDbContext db, IMapper mapper
            ,string? keyword, DateTime? startCreateTime, DateTime? endCreateTime
            ,bool? isCom
            ,int pageNo,int pageSize)
        {
            IQueryable<Logistic> Logistics = db.Logistics;
            if (!string.IsNullOrEmpty(keyword))
            {
                Logistics = Logistics.Where(k=>k.LogisName.Contains(keyword));
            }
            if (startCreateTime.HasValue)
            {
                Logistics = Logistics.Where(k => k.CreateTime >= startCreateTime.Value);
            }
            if (endCreateTime.HasValue)
            {
                Logistics = Logistics.Where(k => k.CreateTime <= endCreateTime.Value);
            }
            var page = await Pagination<Logistic>.CreateAsync(Logistics, pageNo, pageSize);
            page.Items = mapper.Map<List<LogisticDto>>(page.Items);
            return Results.Ok(page);
        }

        [Authorize]
        public static async Task<IResult> Single(AppDbContext db, int id, IMapper mapper, HttpRequest request)
        {
            var et = await db.Logistics.SingleOrDefaultAsync(x => x.Id == id && x.MerchantGuid == request.GetCurrentUser().MerchantGuid);
            return et == null ? Results.NotFound() : Results.Ok(mapper.Map<LogisticDto>(et));
        }

        [Authorize]
        public static async Task<IResult> Edit(AppDbContext db, int id, CreateLogisticDto logisticDto, HttpRequest request, IMapper mapper)
        {
            var et = await db.Logistics.FirstOrDefaultAsync(x => x.Id == id);
            if (et == null)
            {
                return Results.BadRequest("无效的数据");
            }
            if (await db.Logistics.FirstOrDefaultAsync(x => x.Id != id && x.MerchantGuid == request.GetCurrentUser().MerchantGuid && x.LogisName == logisticDto.LogisName) != null)
            {
                return Results.BadRequest("存在相同的物流名称");
            }

            et.LogisName = logisticDto.LogisName;
            et.Remark = logisticDto.Remark;
            et.DefProv= logisticDto.DefProv;
            et.IsDef = logisticDto.IsDef;
            et.LogisAddr = logisticDto.LogisAddr;
            et.IsUsing = logisticDto.IsUsing;
            et.LogisMobile = logisticDto.LogisMobile;
            et.MerchantGuid = request.GetCurrentUser().MerchantGuid;
            await db.SaveChangesAsync();
            return Results.Ok(et);
        }

        [Authorize]
        public static async Task<IResult> Import(AppDbContext db, HttpRequest request,bool isErase = false)
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
                //{ "商品图","PicPath"},
                //{ "商品名称","LogisticName" },
                //{ "商品编码","LogisticNo" },
                //{ "采购价","CostPrice" },
                //{ "销售价","Price" },
                //{ "体积","Volume" },
                //{ "包件数","PackageQty" },
                //{ "采购周期", "PurchaseDays" },
            };
            var (rt, logistics) = Util.ReadExcel<LogisImp>(fs, fieldsMapper);
            if (rt)
            {
                logistics.ForEach(it =>
                {
                    it.Guid = Guid.NewGuid();
                    it.Creator = request.GetCurrentUser().UserName;
                    it.MerchantGuid = request.GetCurrentUser().MerchantGuid;
                    it.CreateTime = DateTime.Now;
                });
                await db.LogisImps.AddRangeAsync(logistics);
                await db.SaveChangesAsync();
                await db.Database.ExecuteSqlRawAsync("p_syncimplogis");
            }
            return Results.Ok(new { isOk = true });
        }


        [Authorize]
        public static async Task<IResult> Delete(AppDbContext db, int id, HttpRequest request)
        {
            var et = await db.Logistics.FirstOrDefaultAsync(x => x.Id == id && x.MerchantGuid == request.GetCurrentUser().MerchantGuid);
            if (et == null)
            {
                return Results.BadRequest("无效的数据");
            }
            await db.SaveChangesAsync();
            return Results.NoContent();
        }
    }
}
