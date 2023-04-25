
using Azure.Core;
using Microsoft.Data.SqlClient;

namespace FurnitureERP.Controllers
{   
    public class PackageController
    {
       [Authorize]
        public static async Task<IResult> Create(AppDbContext db, CreatePackageDto packageDto, HttpRequest request,IMapper mapper)
        {
            if (await db.Packages.FirstOrDefaultAsync(x => x.MerchantGuid == request.GetCurrentUser().MerchantGuid && (x.PackageNo == packageDto.PackageNo || x.PackageName == packageDto.PackageName)) != null)
            {
                return Results.BadRequest("存在相同的包件名称或编码");
            }
            var r = mapper.Map<Package>(packageDto);
            r.Creator = request.GetCurrentUser().UserName;
            r.MerchantGuid = request.GetCurrentUser().MerchantGuid;

            await db.Packages.AddAsync(r);
            await db.SaveChangesAsync();
            return Results.Created($"/package/{r.Id}", r);
        }

        [Authorize]
        public static async Task<IResult> Get(AppDbContext db, IMapper mapper, HttpRequest request)
        {
            var ets = await db.Packages.Where(x=> x.MerchantGuid == request.GetCurrentUser().MerchantGuid).ToListAsync();
            return Results.Ok(mapper.Map<List<PackageDto>>(ets));
        }

        [Authorize]
        public static async Task<IResult> Page(AppDbContext db, IMapper mapper
            ,string? keyword, DateTime? startCreateTime, DateTime? endCreateTime
            ,int pageNo,int pageSize)
        {
            IQueryable<Package> items = db.Packages;
            if (!string.IsNullOrEmpty(keyword))
            {
                items = items.Where(k=>k.PackageName.Contains(keyword) || k.PackageNo.Contains(keyword));
            }
            if (startCreateTime.HasValue)
            {
                items = items.Where(k => k.CreateTime >= startCreateTime.Value);
            }
            if (endCreateTime.HasValue)
            {
                items = items.Where(k => k.CreateTime <= endCreateTime.Value);
            }
            var page = await Pagination<Package>.CreateAsync(items, pageNo, pageSize);
            page.Items = mapper.Map<List<PackageDto>>(page.Items);
            return Results.Ok(page);
        }

        [Authorize]
        public static async Task<IResult> Single(AppDbContext db, long id, IMapper mapper)
        {
            var et = await db.Packages.SingleOrDefaultAsync(x => x.Id == id);
            return et == null ? Results.NotFound() : Results.Ok(mapper.Map<PackageDto>(et));
        }

        [Authorize]
        public static async Task<IResult> Edit(AppDbContext db, long id, CreatePackageDto packageDto, HttpRequest request, IMapper mapper)
        {
            var et = await db.Packages.FirstOrDefaultAsync(x => x.Id == id);
            if (et == null)
            {
                return Results.BadRequest("无效的数据");
            }
            if (await db.Packages.FirstOrDefaultAsync(x => x.Id != id && x.MerchantGuid == request.GetCurrentUser().MerchantGuid && (x.PackageNo == packageDto.PackageNo || x.PackageName == packageDto.PackageName)) != null)
            {
                return Results.BadRequest("存在相同的包件名称或编码");
            }
            et.PackageName = packageDto.PackageName;
            et.Remark = packageDto.Remark;
            et.CostPrice= packageDto.CostPrice;
            et.LengthWidthHeight = packageDto.LengthWidthHeight;
            et.SafeQty = packageDto.SafeQty;
            et.IsUsing = packageDto.IsUsing;
            et.MerchantGuid = request.GetCurrentUser().MerchantGuid;
            await db.SaveChangesAsync();
            return Results.Ok(et);
        }
        
        [Authorize]
        public static async Task<IResult> Delete(AppDbContext db, long id, HttpRequest request)
        {
            var et = await db.Packages.FirstOrDefaultAsync(x => x.Id == id && x.MerchantGuid == request.GetCurrentUser().MerchantGuid);
            if (et == null)
            {
                return Results.BadRequest("无效的数据");
            }
            db.Packages.Remove(et);
            await db.SaveChangesAsync();
            return Results.NoContent();
        }

        [Authorize]
        public static async Task<IResult> Import(AppDbContext db, HttpRequest request, bool isCom = false, bool isErase = false)
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
            return isCom ? await ImportPackageComRelation(db, request, fs) : await ImportPackage(db, request, fs);
        }

        private static async Task<IResult> ImportPackage(AppDbContext db, HttpRequest request, FileStream fs)
        {
            var fieldsMapper = new Dictionary<string, string>() {
                { "包件名称","PackageName"},
                { "包件编码","PackageNo" },
                { "长宽高","LengthWidthHeight" },
                { "体积","Volume" },
                { "采购价","CostPrice" },
                { "采购周期","PurchaseDays" },
            };
            var (rt, items) = Util.ReadExcel<PackageImp>(fs, fieldsMapper);
            if (rt)
            {
                items.ForEach(it =>
                {
                    it.Guid = Guid.NewGuid();
                    it.Creator = request.GetCurrentUser().UserName;
                    it.MerchantGuid = request.GetCurrentUser().MerchantGuid;
                    it.CreateTime = DateTime.Now;
                });
                await db.PackageImps.AddRangeAsync(items);
                await db.SaveChangesAsync();
                await db.Database.ExecuteSqlRawAsync("p_syncimppackage @MerchantGuid"
                    , new SqlParameter("@MerchantGuid", request.GetCurrentUser().MerchantGuid));
            }
            return Results.Ok(new { isOk = true });
        }

        private static async Task<IResult> ImportPackageComRelation(AppDbContext db, HttpRequest request, FileStream fs)
        {
            var fieldsMapper = new Dictionary<string, string>() {
                    { "商品编码","ItemNo" },
                    { "包件编码1","PackageNo1"},
                    { "数量1","Num1" },
                    { "包件编码2","PackageNo2"},
                    { "数量2","Num2" },
                    { "包件编码3","PackageNo3"},
                    { "数量3","Num3" },
                    { "包件编码4","PackageNo4"},
                    { "数量4","Num4" },
                    { "包件编码5","PackageNo5"},
                    { "数量5","Num5" },
                    { "包件编码6","PackageNo6"},
                    { "数量6","Num6" },
                    { "包件编码7","PackageNo7"},
                    { "数量7","Num7" },
                    { "包件编码8","PackageNo8"},
                    { "数量8","Num8" },
                    { "包件编码9","PackageNo9"},
                    { "数量9","Num9" },
                    { "包件编码10","PackageNo10"},
                    { "数量10","Num10" }
                };
            var (rt, items) = Util.ReadExcel<ItemPackageImp>(fs, fieldsMapper);
            if (rt)
            {
                items.ForEach(it =>
                {
                    it.Guid = Guid.NewGuid();
                    it.Creator = request.GetCurrentUser().UserName;
                    it.MerchantGuid = request.GetCurrentUser().MerchantGuid;
                    it.CreateTime = DateTime.Now;
                });
                await db.ItemPackageImps.AddRangeAsync(items);
                await db.SaveChangesAsync();

                var importPackageNos = db.Database.SqlQueryRaw<string>(Sql_GetImportPackageNo, new SqlParameter("@MerchantGuid", request.GetCurrentUser().MerchantGuid)).ToList();
                if (importPackageNos.Count > 0)
                {
                    var execlPath = Util.MarkerCell(fs, fieldsMapper.Count, importPackageNos);
                    return Results.BadRequest(new
                    {
                        url = execlPath,
                        msg = "导入表格中存在包件表中不存在的包件，已对不存在或错误包件编码标红处理，请修改或者添加好商品或者包件信息再进行导入！"
                    });
                }
                else
                {
                    await db.Database.ExecuteSqlRawAsync("p_syncimpitempackage @MerchantGuid"
                    , new SqlParameter("@MerchantGuid", request.GetCurrentUser().MerchantGuid)
                    );
                }
            }
            return Results.Ok(new { isOk = true });
        }

        public static string Sql_GetImportPackageNo => @";with t as(
          SELECT s.ItemNo, d.PackageNo,d.Num,s.MerchantGuid
           FROM sub_item_imp s
          CROSS APPLY (VALUES
                        (PackageNo1, Num1)
                       ,(PackageNo2, Num2)
                       ,(PackageNo3, Num3)
                       ,(PackageNo4, Num4)
                       ,(PackageNo5, Num5)
                       ,(PackageNo6, Num6)
                       ,(PackageNo7, Num7)
                       ,(PackageNo8, Num8)
                       ,(PackageNo9, Num9)
                       ,(PackageNo10, Num10)
                      ) d (PackageNo, Num)
		        where s.MerchantGuid = @MerchantGuid
         )
         select [PackageNo] from t where PackageNo not in (select PackageNo from item)
        AND MerchantGuid = @MerchantGuid
        union
         select [ItemNo] from t where ItemNo not in (select ItemNo from item)
        AND MerchantGuid = @MerchantGuid
        ";
    }
}
