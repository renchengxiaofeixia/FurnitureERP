using AutoMapper;
using Azure.Core;
using FurnitureERP.Dtos;
using FurnitureERP.Models;

namespace FurnitureERP.Controllers
{
    public class StorageController
    {
        [Authorize]
        public static async Task<IResult> Create(AppDbContext db, CreateStorageDto storageDto, HttpRequest request, IMapper mapper)
        {
            var storage = mapper.Map<Storage>(storageDto);
            storage.StorageNo = await Util.GetSerialNoAsync(db, request.GetCurrentUser().MerchantGuid, "Storage");
            storage.AggregateAmount = storageDto.ItemDtos.Sum(k => k.CostPrice * k.StorageNum);
            storage.Creator = request.GetCurrentUser().UserName;
            storage.MerchantGuid = request.GetCurrentUser().MerchantGuid;

            if (storageDto.ItemDtos == null || storageDto.ItemDtos.Count < 1)
            {
                return Results.BadRequest();
            }
            var storageItems = mapper.Map<List<StorageItem>>(storageDto.ItemDtos);
            storageItems.ForEach(pi =>
            {
                pi.CreateTime = DateTime.Now;
                pi.Creator = request.GetCurrentUser().UserName;
                pi.MerchantGuid = storage.MerchantGuid;
                pi.Amount = pi.CostPrice * pi.StorageNum;
                pi.PurchaseNo = storage.PurchaseNo;
                pi.StorageNo = storage.StorageNo;
            });

            await db.StorageItems.AddRangeAsync(storageItems);

            await db.Storages.AddAsync(storage);
            await db.SaveChangesAsync();
            return Results.Created($"/storage/{storage.Id}", storage);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="mapper"></param>
        /// <param name="request"></param>
        /// <param name="keyword"></param>
        /// <param name="startCreateTime"></param>
        /// <param name="endCreateTime"></param>
        /// <param name="startStorageDate"></param>
        /// <param name="endStorageDate"></param>
        /// <param name="pageNo">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        [Authorize]
        public static async Task<IResult> Page(AppDbContext db, IMapper mapper, HttpRequest request
            , string? keyword, DateTime? startCreateTime, DateTime? endCreateTime
            , DateTime? startStorageDate, DateTime? endStorageDate
            , int pageNo, int pageSize)
        {
            IQueryable<Storage> storages = db.Storages.Where(x => x.MerchantGuid == request.GetCurrentUser().MerchantGuid);
            if (!string.IsNullOrEmpty(keyword))
            {
                storages = storages.Where(k => k.WareName.Contains(keyword) || k.SuppName.Contains(keyword)
                || k.StorageNo.Contains(keyword) || k.PurchaseNo.Contains(keyword));
            }
            if (startCreateTime.HasValue)
            {
                storages = storages.Where(k => k.CreateTime >= startCreateTime.Value);
            }
            if (endCreateTime.HasValue)
            {
                storages = storages.Where(k => k.CreateTime <= endCreateTime.Value);
            }
            if (startStorageDate.HasValue)
            {
                storages = storages.Where(k => k.StorageDate >= startStorageDate.Value);
            }
            if (endStorageDate.HasValue)
            {
                storages = storages.Where(k => k.StorageDate <= endStorageDate.Value);
            }
            var page = await Pagination<Storage>.CreateAsync(storages, pageNo, pageSize);
            page.Items = mapper.Map<List<StorageDto>>(page.Items);
            return Results.Ok(page);
        }

        [Authorize]
        public static async Task<IResult> Single(AppDbContext db, long id, IMapper mapper, HttpRequest request)
        {
            var et = await db.Storages.SingleOrDefaultAsync(x => x.Id == id && x.MerchantGuid == request.GetCurrentUser().MerchantGuid);
            var storageDto = mapper.Map<StorageDto>(et);
            return et == null ? Results.NotFound() : Results.Ok(storageDto);
        }

        [Authorize]
        public static async Task<IResult> Edit(AppDbContext db, long id, CreateStorageDto storageDto, HttpRequest request)
        {
            var et = await db.Storages.FirstOrDefaultAsync(x => x.Id == id && x.MerchantGuid == request.GetCurrentUser().MerchantGuid);
            if (et == null)
            {
                return Results.BadRequest("没有找到入库订单信息!!");
            }

            if (storageDto.ItemDtos == null || storageDto.ItemDtos.Count < 1)
            {
                return Results.BadRequest();
            }

            et.StorageDate = storageDto.StorageDate;
            et.StorageType = storageDto.StorageType;
            et.SuppName = storageDto.SuppName;
            et.WareName = storageDto.WareName;
            et.Remark = storageDto.Remark;
            et.AggregateAmount = storageDto.ItemDtos.Sum(k => k.CostPrice * k.StorageNum);

            var items = storageDto.ItemDtos.Select(k => new StorageItem
            {
                ItemNo = k.ItemNo,
                ItemName = k.ItemName,
                SuppName = k.SuppName,
                Amount = k.CostPrice * k.StorageNum,
                CreateTime = DateTime.Now,
                Creator = request.GetCurrentUser().UserName,
                MerchantGuid = et.MerchantGuid,
                PurchaseNo = et.PurchaseNo,
                PurchaseNum = k.PurchaseNum,
                Remark = k.Remark,
                CostPrice = k.CostPrice,
                StorageNo = et.StorageNo
            }).ToList();
            await db.StorageItems.Where(k => k.StorageNo == et.StorageNo && k.MerchantGuid == et.MerchantGuid).ExecuteDeleteAsync();
            await db.StorageItems.AddRangeAsync(items);
            await db.SaveChangesAsync();
            return Results.Ok(et);
        }

        [Authorize]
        public static async Task<IResult> Delete(AppDbContext db, long id, HttpRequest request)
        {
            var et = await db.Storages.FirstOrDefaultAsync(x => x.Id == id && x.MerchantGuid == request.GetCurrentUser().MerchantGuid);
            if (et == null)
            {
                return Results.BadRequest();
            }
            if (et.IsAudit)
            {
                return Results.BadRequest("已经审核的入库订单信息，不能删除!!");
            }
            await db.StorageItems.Where(k => k.StorageNo == et.StorageNo).ExecuteDeleteAsync();
            db.Storages.Remove(et);
            await db.SaveChangesAsync();
            return Results.NoContent();
        }

        [Authorize]
        public static async Task<IResult> Audit(AppDbContext db, long id, HttpRequest request, IMapper mapper)
        {
            var et = await db.Storages.FirstOrDefaultAsync(x => x.Id == id && x.MerchantGuid == request.GetCurrentUser().MerchantGuid);
            if (et == null)
            {
                return Results.BadRequest("没有找到入库订单信息!!");
            }
            if (et.IsAudit)
            {
                return Results.BadRequest("入库订单已经审核!!");
            }
            try
            {
                db.Database.BeginTransaction();
                if (!string.IsNullOrEmpty(et.PurchaseNo) && et.StorageType == "采购入库")
                {
                    //采购入库，更新采购单的入库数量
                    var purchaseForUpdate = from p in db.PurchaseItems
                                            join e in db.StorageItems
                                            on p.ItemNo equals e.ItemNo
                                            where p.PurchaseNo == et.PurchaseNo && e.StorageNo == et.StorageNo
                                            select new { p, e };
                    await purchaseForUpdate.ForEachAsync(up => up.p.StorageNum += up.e.StorageNum);


                    ////采购入库，更新库存数量
                    var inventoryForUpdate = from p in db.Inventories
                                             join e in db.StorageItems
                                             on p.Guid equals e.Guid
                                             //on p.ItemNo equals e.ItemNo
                                             //where p.StorageNo == et.StorageNo && p.WareName == et.WareName
                                             select new { p, e };
                    await inventoryForUpdate.ForEachAsync(up => up.p.Quantity += up.e.StorageNum);

                    //采购入库，插入库存数量
                    var storageItemForInsert = await (from p in db.StorageItems
                                                      join e in db.Inventories
                                                      on p.Guid equals e.Guid
                                                      //on new { p.ItemNo, et.WareName, p.StorageNo } equals new { e.ItemNo, e.WareName, et.StorageNo }
                                                      into grp
                                                      from g in grp.DefaultIfEmpty()
                                                      select p).ToListAsync();
                    var inventoryForInsert = mapper.Map<List<Inventory>>(storageItemForInsert);
                    inventoryForInsert.ForEach(ivt =>
                    {
                        ivt.Id = 0;
                        ivt.SuppName = et.SuppName;
                        ivt.PurchaseNo = et.PurchaseNo;
                        ivt.WareName = et.WareName;
                        ivt.StorageTime = et.StorageDate;
                        ivt.StorageType = et.StorageType;
                        ivt.CreateTime = DateTime.Now;
                        ivt.Creator = request.GetCurrentUser().UserName;
                        ivt.Tid = et.Tid;
                    });
                    await db.Inventories.AddRangeAsync(inventoryForInsert);

                    //包件入库

                }
                else if (et.StorageType == "其他入库")
                {
                    //其他入库，插入库存数量
                    var storageItems = await db.StorageItems.Where(k => k.StorageNo == et.StorageNo).ToListAsync();
                    var inventoryForInsert = mapper.Map<List<Inventory>>(storageItems);
                    inventoryForInsert.ForEach(ivt =>
                    {
                        ivt.Id = 0;
                        ivt.WareName = et.WareName;
                        ivt.StorageTime = et.StorageDate;
                        ivt.StorageType = et.StorageType;
                        ivt.CreateTime = DateTime.Now;
                        ivt.Creator = request.GetCurrentUser().UserName;
                    });
                    await db.Inventories.AddRangeAsync(inventoryForInsert);

                    //包件入库
                    var packageForInsert = await (from p in db.StorageItems
                                                  from k in db.ItemPackages
                                                  from j in db.Packages
                                                  where p.ItemNo == k.ItemNo && k.PackageNo == j.PackageNo
                                                  select new Inventorybarcode
                                                  {
                                                      Guid = Guid.NewGuid(),
                                                      PackageName = j.PackageName,
                                                      PackageNo = j.PackageNo,
                                                      ItemName = p.ItemName,
                                                      ItemNo = p.ItemNo,
                                                      BarCode = string.Empty,
                                                      StdItemNo = p.StdItemNo,
                                                      StorageNo = et.StorageNo,
                                                      PurchaseNo = et.PurchaseNo,
                                                      SuppName = et.SuppName,
                                                      Tid = et.Tid,
                                                      WareName = et.WareName,
                                                      PackageQuantity = p.StorageNum,
                                                      InventoryGuid = p.Guid
                                                  }).ToListAsync();
                    await db.Inventorybarcodes.AddRangeAsync(packageForInsert);

                }
                et.IsAudit = true;
                et.AuditDate = DateTime.Now;
                et.AuditUser = request.GetCurrentUser().UserName;
                await db.SaveChangesAsync();
                await db.Database.CommitTransactionAsync();
            }
            catch (Exception ex) { }
            return Results.Ok(et);
        }

        [Authorize]
        public static async Task<IResult> UnAudit(AppDbContext db, long id, HttpRequest request)
        {
            var et = await db.Storages.FirstOrDefaultAsync(x => x.Id == id && x.MerchantGuid == request.GetCurrentUser().MerchantGuid);
            if (et == null)
            {
                return Results.BadRequest("没有找到入库订单信息!!");
            }
            //退审核检查
            if (!(from p in db.Inventories
                  join e in db.StorageItems
                  on p.Guid equals e.Guid
                  //on new { p.StorageNo, p.ItemNo } equals new { e.StorageNo, e.ItemNo }
                  where p.WareName == et.WareName && p.Quantity >= e.StorageNum
                  select e).Any())
            {
                return Results.BadRequest("不能退审核此入库订单，此入库单的商品在库存里已经被修改!!");
            }

            //入库退审，扣减库存数
            var inventoryForUpdate = from p in db.Inventories
                                     join e in db.StorageItems
                                     on p.Guid equals e.Guid
                                     select new { p, e };
            await inventoryForUpdate.ForEachAsync(up =>
            {
                up.p.Quantity -= up.e.StorageNum;
            });

            et.IsAudit = false;
            et.AuditDate = null;
            et.AuditUser = string.Empty;
            await db.SaveChangesAsync();
            return Results.Ok(et);
        }

        [Authorize]
        public static async Task<IResult> GetStorageProdInfos(AppDbContext db, long id, IMapper mapper, HttpRequest request)
        {
            var et = await db.Storages.FirstOrDefaultAsync(x => x.Id == id && x.MerchantGuid == request.GetCurrentUser().MerchantGuid);
            if (et == null)
            {
                return Results.BadRequest("没有找到入库订单信息!!");
            }
            var items = await db.StorageItems.Where(k => k.StorageNo == et.StorageNo).ToListAsync();
            //items = items.OrderByDescending(s => s.Id).ToList();

            return Results.Ok(mapper.Map<List<StorageItemDto>>(items));
        }
    }
}
