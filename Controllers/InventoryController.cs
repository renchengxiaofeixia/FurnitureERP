﻿using AutoMapper;
using FurnitureERP.Dtos;
using FurnitureERP.Enums;
using FurnitureERP.Models;

namespace FurnitureERP.Controllers
{
    public class InventoryController
    {
        [Authorize]
        public static async Task<IResult> GetInventories(AppDbContext db, IMapper mapper, HttpRequest request
            , string? keyword, int pageNo, int pageSize)
        {
            IQueryable<Inventory> inventories = db.Inventories.Where(x=>x.Quantity > 0 && x.MerchantGuid == request.GetCurrentUser().MerchantGuid);
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
            var inventoryIds = inventoryItemAdjustDtos.Select(k => k.Guid);
            var inventories = await db.Inventories.Where(k => inventoryIds.Any(id=> id == k.Guid)).ToListAsync();
            if ((from k in inventories
                 from j in inventoryItemAdjustDtos
                 where k.Guid == j.Guid && k.Quantity != j.Quantity
                 select k).Any())
            {
                return Results.BadRequest("选择的库存商品数量不匹配!!");
            }

            var adjustNo = await Util.GetSerialNoAsync(db, request.GetCurrentUser().MerchantGuid, "InventoryAdjust");
            try
            {
                db.Database.BeginTransaction();

                if (Params.Inventory.Dimension(request.GetCurrentUser().MerchantGuid) == InventoryDimensionEnum.Package)
                {
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

                    //更新库存
                    var inventoryForUpdate = from p in db.InventoryPackages
                                             join e in db.InventoryItemAdjusts
                                             on p.Guid equals e.Guid
                                             select new { p, e };
                    await inventoryForUpdate.ForEachAsync(up =>
                    {
                        up.p.Quantity = up.e.AdjustQuantity;
                    });
                }
                else if (Params.Inventory.Dimension(request.GetCurrentUser().MerchantGuid) == InventoryDimensionEnum.Item)
                {
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

                    //更新库存
                    var inventoryForUpdate = from p in db.Inventories
                                             join e in db.InventoryItemAdjusts
                                             on p.Guid equals e.Guid
                                             select new { p, e };
                    await inventoryForUpdate.ForEachAsync(up =>
                    {
                        up.p.Quantity = up.e.AdjustQuantity;
                    });

                }

                await db.SaveChangesAsync();
                db.Database.CommitTransaction();
            }
            catch { 
            }
            return Results.Ok();
        }

        [Authorize]
        public static async Task<IResult> MoveInventoryItems(AppDbContext db, List<InventoryItemMoveDto> inventoryItemMoveDtos, HttpRequest request,IMapper mapper)
        {
            var inventoryIds = inventoryItemMoveDtos.Select(k=>k.Guid);
            var inventories = await db.Inventories.Where(k => inventoryIds.Any(id => id == k.Guid)).ToListAsync();
            if ((from k in inventories
                 from j in inventoryItemMoveDtos
                 where k.Guid == j.Guid && k.Quantity != j.Quantity
                 select k).Any())
            {
                return Results.BadRequest("选择的库存商品数量不匹配!!");
            }
            var moveNo = await Util.GetSerialNoAsync(db, request.GetCurrentUser().MerchantGuid, "InventoryMove");
            try
            {
                await db.Database.BeginTransactionAsync();
                if (Params.Inventory.Dimension(request.GetCurrentUser().MerchantGuid) == InventoryDimensionEnum.Item)
                {
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

                    //更新库存
                    var inventoryForUpdate = from p in db.Inventories
                                             join e in db.InventoryItemMoves
                                             on p.Guid equals e.Guid
                                             select new { p, e };
                    await inventoryForUpdate.ForEachAsync(up =>
                    {
                        up.p.Quantity -= up.e.MoveQuantity;
                    });

                    //插入移动到新的仓库的库存
                    inventoryItemMoveDtos.ForEach(k => k.WareName = k.ToWareName);
                    var inventoryForInsert = mapper.Map<List<Inventory>>(inventoryItemMoveDtos);
                    inventoryForInsert.ForEach(ivt =>
                    {
                        ivt.StorageNo = moveNo;
                    });
                    await db.Inventories.AddRangeAsync(inventoryForInsert);
                }
                else if (Params.Inventory.Dimension(request.GetCurrentUser().MerchantGuid) == InventoryDimensionEnum.Package)
                {
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

                    //更新库存
                    var inventoryForUpdate = from p in db.InventoryPackages
                                             join e in db.InventoryItemMoves
                                             on p.Guid equals e.Guid
                                             select new { p, e };
                    await inventoryForUpdate.ForEachAsync(up =>
                    {
                        up.p.Quantity -= up.e.MoveQuantity;
                    });

                    //插入移动到新的仓库的库存
                    inventoryItemMoveDtos.ForEach(k => k.WareName = k.ToWareName);
                    var inventoryForInsert = mapper.Map<List<InventoryPackage>>(inventoryItemMoveDtos);
                    inventoryForInsert.ForEach(ivt =>
                    {
                        ivt.StorageNo = moveNo;
                    });
                    await db.InventoryPackages.AddRangeAsync(inventoryForInsert);
                }

                await db.SaveChangesAsync();

                await db.Database.CommitTransactionAsync();
            }
            catch
            {
            }
            return Results.Ok();
        }
    }
}
