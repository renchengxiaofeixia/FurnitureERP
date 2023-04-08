
using Azure.Core;
using Microsoft.Data.SqlClient;

namespace FurnitureERP.Controllers
{
   
    public class ItemController
    {       

       [Authorize]
        public static async Task<IResult> Create(AppDbContext db, CreateItemDto itemDto, HttpRequest request,IMapper mapper)
        {
            if (await db.Items.FirstOrDefaultAsync(x => x.MerchantGuid == request.GetCurrentUser().MerchantGuid && (x.ItemNo == itemDto.ItemNo || x.ItemName == itemDto.ItemName)) != null)
            {
                return Results.BadRequest("存在相同的商品名称或编码");
            }
            var r = mapper.Map<Item>(itemDto);
            r.Creator = request.GetCurrentUser().UserName;
            r.MerchantGuid = request.GetCurrentUser().MerchantGuid;
            if (itemDto.SubItems != null && itemDto.SubItems.Count > 0)
            {
                var subItems = mapper.Map<List<SubItem>>(itemDto.SubItems);
                subItems.ForEach(si =>
                {
                    si.Creator = request.GetCurrentUser().UserName;
                    si.MerchantGuid = request.GetCurrentUser().MerchantGuid;
                });
                await db.SubItems.AddRangeAsync(subItems);
                //标记成组合商品
                r.IsCom = true;
            }
            await db.Items.AddAsync(r);
            await db.SaveChangesAsync();
            return Results.Created($"/item/{r.Id}", r);
        }

        [Authorize]
        public static async Task<IResult> Get(AppDbContext db, IMapper mapper, HttpRequest request)
        {
            var ets = await db.Items.Where(x=> x.MerchantGuid == request.GetCurrentUser().MerchantGuid).ToListAsync();
            return Results.Ok(mapper.Map<List<ItemDto>>(ets));
        }

        [Authorize]
        public static async Task<IResult> GetSubItems(AppDbContext db, IMapper mapper,int id, HttpRequest request)
        {
            if (!await db.Items.AnyAsync(x => x.Id == id && x.MerchantGuid == request.GetCurrentUser().MerchantGuid))
            {
                return Results.BadRequest("无效的数据");
            }
            var ets = from it in db.Items
                      from si in db.SubItems
                      where it.ItemNo == si.ItemNo && it.Id == id && si.MerchantGuid == request.GetCurrentUser().MerchantGuid  
                      select it;
            return Results.Ok(mapper.Map<List<ItemDto>>(await ets.ToListAsync()));
        }

        [Authorize]
        public static async Task<IResult> Page(AppDbContext db, IMapper mapper
            ,string? keyword, DateTime? startCreateTime, DateTime? endCreateTime
            ,bool? isCom
            ,int pageNo,int pageSize)
        {
            IQueryable<Item> items = db.Items;
            if (!string.IsNullOrEmpty(keyword))
            {
                items = items.Where(k=>k.ItemName.Contains(keyword) || k.ItemNo.Contains(keyword));
            }
            if (startCreateTime.HasValue)
            {
                items = items.Where(k => k.CreateTime >= startCreateTime.Value);
            }
            if (endCreateTime.HasValue)
            {
                items = items.Where(k => k.CreateTime <= endCreateTime.Value);
            }
            if (isCom.HasValue) {
                items = items.Where(k => k.IsCom == isCom.Value) ;
            }
            var page = await Pagination<Item>.CreateAsync(items, pageNo, pageSize);
            page.Items = mapper.Map<List<ItemDto>>(page.Items);
            return Results.Ok(page);
        }

        [Authorize]
        public static async Task<IResult> Single(AppDbContext db, int id, IMapper mapper)
        {
            var et = await db.Items.SingleOrDefaultAsync(x => x.Id == id);
            return et == null ? Results.NotFound() : Results.Ok(mapper.Map<ItemDto>(et));
        }

        [Authorize]
        public static async Task<IResult> Edit(AppDbContext db, int id, CreateItemDto itemDto, HttpRequest request, IMapper mapper)
        {
            var et = await db.Items.FirstOrDefaultAsync(x => x.Id == id);
            if (et == null)
            {
                return Results.BadRequest("无效的数据");
            }
            if (await db.Items.FirstOrDefaultAsync(x => x.Id != id && x.MerchantGuid == request.GetCurrentUser().MerchantGuid && (x.ItemNo == itemDto.ItemNo || x.ItemName == itemDto.ItemName)) != null)
            {
                return Results.BadRequest("存在相同的商品名称或编码");
            }
            if (itemDto.SubItems != null && itemDto.SubItems.Count > 0)
            {
                //删除组合商品下的子商品
                var removeSubItems = await db.SubItems.Where(k => k.ItemNo == et.ItemNo && k.MerchantGuid == request.GetCurrentUser().MerchantGuid).ToListAsync();
                db.SubItems.RemoveRange(removeSubItems);
                var subItems = mapper.Map<List<SubItem>>(itemDto.SubItems);
                subItems.ForEach(si =>
                {
                    si.Creator = request.GetCurrentUser().UserName;
                    si.MerchantGuid = request.GetCurrentUser().MerchantGuid;
                });
                await db.SubItems.AddRangeAsync(subItems);

                //标记成组合商品
                et.IsCom = true;
            }

            et.ItemName = itemDto.ItemName;
            et.Remark = itemDto.Remark;
            et.CostPrice= itemDto.CostPrice;
            et.PicPath = itemDto.PicPath;
            et.Price = itemDto.Price;
            et.IsUsing = itemDto.IsUsing;
            et.IsCom = itemDto.IsCom;
            et.MerchantGuid = request.GetCurrentUser().MerchantGuid;
            await db.SaveChangesAsync();
            return Results.Ok(et);
        }

        [Authorize]
        public static async Task<IResult> Upload(AppDbContext db, HttpRequest request, int id)
        {
            if (!request.HasFormContentType)
                return Results.BadRequest();
            var et = await db.Items.FirstOrDefaultAsync(x => x.Id == id && x.MerchantGuid == request.GetCurrentUser().MerchantGuid);
            if (et == null)
            {
                return Results.BadRequest("无效的数据");
            }

            var form = await request.ReadFormAsync();
            var fi = form.Files["fi"];
            if (fi is null || fi.Length == 0)
                return Results.BadRequest();

            if (!Path.GetExtension(fi.FileName).EndsWith("jpg") && !Path.GetExtension(fi.FileName).EndsWith("png"))
                return Results.BadRequest("图片只支持jpg和png");

            var svrFn = $"{DateTime.Now.Ticks}{Path.GetExtension(fi.FileName)}";
            var svrpath = Path.Combine(AppContext.BaseDirectory, "images", svrFn);
            await using var stream = fi.OpenReadStream();
            using var fs = File.Create(svrpath);
            await stream.CopyToAsync(fs);
            et.PicPath = $"/images/{svrFn}";
            await db.SaveChangesAsync();
            return Results.Ok(et);
        }

        [Authorize]
        public static async Task<IResult> Import(AppDbContext db, HttpRequest request,bool isCom = false,bool isErase = false)
        {
            if (!request.HasFormContentType)
                return Results.BadRequest();

            var form = await request.ReadFormAsync();
            var fi = form.Files["fi"];
            if (fi is null || fi.Length == 0)
                return Results.BadRequest();
            if(!Path.GetExtension(fi.FileName).EndsWith("xlsx"))
                return Results.BadRequest("文件格式错误");
            var svrFn = $"{DateTime.Now.Ticks}{Path.GetExtension(fi.FileName)}";
            var svrpath = Path.Combine(AppContext.BaseDirectory, "excel", svrFn);
            await using var stream = fi.OpenReadStream();
            using var fs = File.Create(svrpath);
            await stream.CopyToAsync(fs); 



            return isCom ? await ImportItemComRelation(db, request, fs) : await ImportItem(db, request, fs);
        }

        private static async Task<IResult> ImportItemComRelation(AppDbContext db, HttpRequest request, FileStream fs)
        {
            var fieldsMapper = new Dictionary<string, string>() {
                    { "商品编码","ItemNo" },
                    { "子商品编码1","SubItemNo1"},
                    { "数量1","Num1" },
                    { "子商品编码2","SubItemNo2"},
                    { "数量2","Num2" },
                    { "子商品编码3","SubItemNo3"},
                    { "数量3","Num3" },
                    { "子商品编码4","SubItemNo4"},
                    { "数量4","Num4" },
                    { "子商品编码5","SubItemNo5"},
                    { "数量5","Num5" },
                    { "子商品编码6","SubItemNo6"},
                    { "数量6","Num6" },
                    { "子商品编码7","SubItemNo7"},
                    { "数量7","Num7" },
                    { "子商品编码8","SubItemNo8"},
                    { "数量8","Num8" },
                    { "子商品编码9","SubItemNo9"},
                    { "数量9","Num9" },
                    { "子商品编码10","SubItemNo10"},
                    { "数量10","Num10" }
                };

            //var itemNos = await (from it in db.Items
            //                     select it.ItemNo).ToListAsync();

           

            var (rt, items) = Util.ReadExcel<SubItemImp>(fs, fieldsMapper);
            if (rt)
            {
                items.ForEach(it => {
                    it.Guid = Guid.NewGuid();
                    it.Creator = request.GetCurrentUser().UserName;
                    it.CreateTime = DateTime.Now;
                    it.MerchantGuid = request.GetCurrentUser().MerchantGuid;
                });
                await db.SubItemImps.AddRangeAsync(items);
                await db.SaveChangesAsync();

                //获取cell
                var cellValues = db.Database.SqlQueryRaw<string>(GetCellSql, new SqlParameter("@MerchantGuid", request.GetCurrentUser().MerchantGuid)).ToList();


                if (cellValues.Count > 0)
                {
                    var execlPath = Util.CheckCellValues(fs, fieldsMapper.Count, cellValues);

                    return Results.BadRequest(new { 
                        url = execlPath,
                        msg = "导入表格中存在商品表中不存在的商品，已对不存在或错误商品编码标红处理，请修改或者添加好商品再进行组合产品导入！"
                    });
                }
                else
                {
                    await db.Database.ExecuteSqlRawAsync("p_syncimpsubitem @MerchantGuid"
                     , new SqlParameter("@MerchantGuid", request.GetCurrentUser().MerchantGuid)
                    );
                }

            

                
            }
            return Results.Ok(new { isOk = true });
        }

        private static async Task<IResult> ImportItem(AppDbContext db, HttpRequest request, FileStream fs)
        {
            var fieldsMapper = new Dictionary<string, string>() {
                { "商品图","PicPath"},
                { "商品名称","ItemName" },
                { "商品编码","ItemNo" },
                { "供应商","SuppName" },
                { "采购价","CostPrice" },
                { "销售价","Price" },
                { "体积","Volume" },
                { "包件数","PackageQty" },
                { "采购周期", "PurchaseDays" },
            };
            var (rt, items) = Util.ReadExcel<ItemImp>(fs, fieldsMapper, "商品图");
            if (rt)
            {
                items.ForEach(it =>
                {
                    it.Guid = Guid.NewGuid();
                    it.Creator = request.GetCurrentUser().UserName;
                    it.MerchantGuid = request.GetCurrentUser().MerchantGuid;
                    it.CreateTime = DateTime.Now;
                });
                await db.ItemImps.AddRangeAsync(items);
                await db.SaveChangesAsync();
                //await db.Database.ExecuteSqlRawAsync($"p_syncimpitem '{request.GetCurrentUser().MerchantGuid}'");
                await db.Database.ExecuteSqlRawAsync("p_syncimpitem @MerchantGuid"
                    ,new SqlParameter("@MerchantGuid", request.GetCurrentUser().MerchantGuid)
                    );

            }
            return Results.Ok(new { isOk = true });
        }

        [Authorize]
        public static async Task<IResult> Delete(AppDbContext db, int id, HttpRequest request)
        {
            var et = await db.Items.FirstOrDefaultAsync(x => x.Id == id && x.MerchantGuid == request.GetCurrentUser().MerchantGuid);
            if (et == null)
            {
                return Results.BadRequest("无效的数据");
            }
            if (et.IsCom)
            {
                var removeSubItems = await db.SubItems.Where(k => k.ItemNo == et.ItemNo).ToListAsync();
                db.SubItems.RemoveRange(removeSubItems);
            }
            await db.SaveChangesAsync();
            return Results.NoContent();
        }

        public static string GetCellSql => @";with t as(
          SELECT s.ItemNo, d.SubItemNo,d.Num,s.MerchantGuid
           FROM sub_item_imp s
          CROSS APPLY (VALUES
                        (SubItemNo1, Num1)
                       ,(SubItemNo2, Num2)
                       ,(SubItemNo3, Num3)
                       ,(SubItemNo4, Num4)
                       ,(SubItemNo5, Num5)
                       ,(SubItemNo6, Num6)
                       ,(SubItemNo7, Num7)
                       ,(SubItemNo8, Num8)
                       ,(SubItemNo9, Num9)
                       ,(SubItemNo10, Num10)
                      ) d (SubItemNo, Num)
		        where s.MerchantGuid = @MerchantGuid
         )
         select [SubItemNo] from t where SubItemNo not in (select ItemNo from item)
        AND MerchantGuid = @MerchantGuid
        union
         select [ItemNo] from t where ItemNo not in (select ItemNo from item)
        AND MerchantGuid = @MerchantGuid
        ";
    }
}
