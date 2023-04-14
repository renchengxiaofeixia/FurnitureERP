using AutoMapper;
using FurnitureERP.Dtos;
using FurnitureERP.Models;

namespace FurnitureERP.Controllers
{
    public class InventoryController
    {
        [Authorize]
        public static async Task<IResult> GetInventories(AppDbContext db, IMapper mapper, HttpRequest request
            , string? keyword, int pageNo, int pageSize)
        {
            IQueryable<Inventory> inventories = db.Inventories.Where(x=>x.MerchantGuid == request.GetCurrentUser().MerchantGuid);
            if (!string.IsNullOrEmpty(keyword))
            {
                inventories = inventories.Where(k => k.WareName.Contains(keyword) 
                || k.SuppName.Contains(keyword) || k.PurchaseNo.Contains(keyword)
                || k.ItemNo.Contains(keyword) || k.StorageNo.Contains(keyword));
            }
            var page = await Pagination<Inventory>.CreateAsync(inventories, pageNo, pageSize);
            page.Items = mapper.Map<List<InventoryDto>>(page.Items);
            return Results.Ok(page);
        }

        [Authorize]
        public static async Task<IResult> GetInventoriesCountForWareName(AppDbContext db, IMapper mapper, HttpRequest request)
        {
            var ets = await db.Inventories.FirstOrDefaultAsync(x => x.MerchantGuid == request.GetCurrentUser().MerchantGuid);
            return Results.Ok(mapper.Map<List<InventoryDto>>(ets));
        }

        [Authorize]
        public static async Task<IResult> AdjustInventoryItems(AppDbContext db, List<InventoryItemAdjustDto> inventoryItemAdjustDtos, HttpRequest request,IMapper mapper)
        {
            var inventories = await db.Inventories.Where(k => inventoryItemAdjustDtos.Any(j=>j.Guid == k.Guid)).ToListAsync();
            if ((from k in inventories
                 from j in inventoryItemAdjustDtos
                 where k.Guid == j.Guid && k.Quantity != j.Quantity
                 select k).Any())
            {
                return Results.BadRequest("选择的库存商品数量不匹配!!");
            }

            var adjustNo = await Util.GetSerialNoAsync(db, request.GetCurrentUser().MerchantGuid, "InventoryAdjust");

            //更新库存
            var inventoryForUpdate = from p in db.Inventories
                                    join e in inventoryItemAdjustDtos
                                    on p.Guid equals e.Guid
                                    select new { p, e };
            await inventoryForUpdate.ForEachAsync(up =>
            {
                up.p.Quantity = up.e.AdjustQuantity;
            });

            //插入库存调整记录
            var inventoryItemAdjusts = mapper.Map<List<InventoryItemAdjust>>(inventoryItemAdjustDtos);
            inventoryItemAdjusts.ForEach(k =>
            {
                k.Creator = request.GetCurrentUser().UserName;
                k.CreateTime = DateTime.Now;
                k.MerchantGuid = request.GetCurrentUser().MerchantGuid;
                k.AdjustNo = adjustNo;
            });
            await db.InventoryItemAdjusts.AddRangeAsync(inventoryItemAdjusts);
            await db.SaveChangesAsync();
            return Results.Ok();
        }

        [Authorize]
        public static async Task<IResult> MoveInventoryItems(AppDbContext db, List<InventoryItemMoveDto> inventoryItemMoveDtos, HttpRequest request,IMapper mapper)
        {
            var inventories = await db.Inventories.Where(k => inventoryItemMoveDtos.Any(j => j.Guid == k.Guid)).ToListAsync();
            if ((from k in inventories
                 from j in inventoryItemMoveDtos
                 where k.Guid == j.Guid && k.Quantity != j.Quantity
                 select k).Any())
            {
                return Results.BadRequest("选择的库存商品数量不匹配!!");
            }
            var moveNo = await Util.GetSerialNoAsync(db, request.GetCurrentUser().MerchantGuid, "InventoryMove");

            //更新库存
            var inventoryForUpdate = from p in db.Inventories
                                    join e in inventoryItemMoveDtos
                                    on p.Guid equals e.Guid
                                    select new { p, e };
            await inventoryForUpdate.ForEachAsync(up =>
            {
                up.p.Quantity -= up.e.MoveQuantity;
            });

            //插入移动到新的仓库的库存
            inventoryItemMoveDtos.ForEach(k=>k.WareName = k.ToWareName);
            var inventoryForInsert = mapper.Map<List<Inventory>>(inventoryItemMoveDtos);
            inventoryForInsert.ForEach(ivt =>
            {
                ivt.StorageNo = moveNo;
            });
            await db.Inventories.AddRangeAsync(inventoryForInsert);

            //保存库存移动记录
            var inventoryItemMoves = mapper.Map<List<InventoryItemMove>>(inventoryItemMoveDtos);
            inventoryItemMoves.ForEach(k =>
            {
                k.Creator = request.GetCurrentUser().UserName;
                k.CreateTime = DateTime.Now;
                k.MerchantGuid = request.GetCurrentUser().MerchantGuid;
                k.MoveNo = moveNo;

            });
            await db.InventoryItemMoves.AddRangeAsync(inventoryItemMoves);
            await db.SaveChangesAsync();
            return Results.Ok();
        }
    }
}
