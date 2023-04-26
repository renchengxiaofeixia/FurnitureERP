using Microsoft.AspNetCore.Authorization;
using FurnitureERP.Database;
using FurnitureERP.Dtos;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using FurnitureERP.Models;
using FurnitureERP.Utils;
using Microsoft.AspNetCore.Http.HttpResults;
using Azure.Core;

namespace FurnitureERP.Controllers
{
    public class PurchaseController
    {
        [Authorize]
        public static async Task<IResult> Create(AppDbContext db, CreatePurchaseDto purchaseDto, HttpRequest request, IMapper mapper)
        {
            var purchase = mapper.Map<Purchase>(purchaseDto);
            purchase.PurchaseNo = await Util.GetSerialNoAsync(db, request.GetCurrentUser().MerchantGuid, "purchase");
            purchase.AggregateAmount = purchaseDto.ItemDtos.Sum(k => k.CostPrice * k.PurchaseNum);
            purchase.Creator = request.GetCurrentUser().UserName;
            purchase.MerchantGuid = request.GetCurrentUser().MerchantGuid;

            if (purchaseDto.ItemDtos == null || purchaseDto.ItemDtos.Count < 1)
            {
                return Results.BadRequest();
            }
            var purchasePackages = purchaseDto.ItemDtos.SelectMany(k => {
                k.Guid = Guid.NewGuid();
                if (k.PackageDtos == null) return null;
                var dtos = mapper.Map<List<PurchasePackage>>(k.PackageDtos);
                dtos.ForEach(p => {
                    p.Amount = p.CostPrice * p.PurchaseNum;
                    p.CreateTime = DateTime.Now;
                    p.Creator = request.GetCurrentUser().UserName;
                    p.MerchantGuid = purchase.MerchantGuid;
                    p.PurchaseNo = purchase.PurchaseNo;
                    p.PurchaseItemGuid = k.Guid;
                });
                return dtos;
            });

            var purchaseItems = mapper.Map<List<PurchaseItem>>(purchaseDto.ItemDtos);
            purchaseItems.ForEach(pi =>
            {
                pi.Amount = pi.CostPrice * pi.PurchaseNum;
                pi.CreateTime = DateTime.Now;
                pi.Creator = request.GetCurrentUser().UserName;
                pi.MerchantGuid = purchase.MerchantGuid;
                pi.PurchaseNo = purchase.PurchaseNo;
            });

            if (purchasePackages != null)
            {
                await db.PurchasePackages.AddRangeAsync(purchasePackages);
            }
            await db.PurchaseItems.AddRangeAsync(purchaseItems);
            await db.Purchases.AddAsync(purchase);
            await db.SaveChangesAsync();
            return Results.Created($"/purchase/{purchase.Id}", purchase);
        }

        [Authorize]
        public static async Task<IResult> Page(AppDbContext db, IMapper mapper, HttpRequest request
            , string? keyword, DateTime? startCreateTime, DateTime? endCreateTime
            , DateTime? startPurchaseOrderDate, DateTime? endPurchaseOrderDate
            , DateTime? startDeliveryDate, DateTime? endDeliveryDate
            , int pageNo, int pageSize)
        {
            IQueryable<Purchase> purchs = db.Purchases.Where(x => x.MerchantGuid == request.GetCurrentUser().MerchantGuid);
            if (!string.IsNullOrEmpty(keyword))
            {
                purchs = purchs.Where(k => k.WareName.Contains(keyword) || k.SuppName.Contains(keyword) || k.PurchaseNo.Contains(keyword));
            }
            if (startCreateTime.HasValue)
            {
                purchs = purchs.Where(k => k.CreateTime >= startCreateTime.Value);
            }
            if (endCreateTime.HasValue)
            {
                purchs = purchs.Where(k => k.CreateTime <= endCreateTime.Value);
            }
            if (startPurchaseOrderDate.HasValue)
            {
                purchs = purchs.Where(k => k.PurchaseOrderDate >= startPurchaseOrderDate.Value);
            }
            if (endPurchaseOrderDate.HasValue)
            {
                purchs = purchs.Where(k => k.PurchaseOrderDate <= endPurchaseOrderDate.Value);
            }
            if (startDeliveryDate.HasValue)
            {
                purchs = purchs.Where(k => k.DeliveryDate >= startDeliveryDate.Value);
            }
            if (endDeliveryDate.HasValue)
            {
                purchs = purchs.Where(k => k.DeliveryDate <= endDeliveryDate.Value);
            }
            var page = await Pagination<Purchase>.CreateAsync(purchs, pageNo, pageSize);
            page.Items = mapper.Map<List<PurchaseDto>>(page.Items);
            return Results.Ok(page);
        }

        [Authorize]
        public static async Task<IResult> Single(AppDbContext db, int id, IMapper mapper, HttpRequest request)
        {
            var et = await db.Purchases.SingleOrDefaultAsync(x => x.Id == id && x.MerchantGuid == request.GetCurrentUser().MerchantGuid);
            var purchaseDto = mapper.Map<PurchaseDto>(et);
            return et == null ? Results.NotFound() : Results.Ok(purchaseDto);
        }

        [Authorize]
        public static async Task<IResult> Edit(AppDbContext db, int id, CreatePurchaseDto purchaseDto, HttpRequest request, IMapper mapper)
        {
            var et = await db.Purchases.FirstOrDefaultAsync(x => x.Id == id && x.MerchantGuid == request.GetCurrentUser().MerchantGuid);
            if (et == null)
            {
                return Results.BadRequest("没有找到采购订单信息!!");
            }

            if (purchaseDto.ItemDtos == null || purchaseDto.ItemDtos.Count < 1)
            {
                return Results.BadRequest();
            }

            et.PurchaseOrderDate = purchaseDto.PurchaseOrderDate;
            et.SettlementMode = purchaseDto.SettlementMode;
            et.SuppName = purchaseDto.SuppName;
            et.WareName = purchaseDto.WareName;
            et.DeliveryDate = purchaseDto.DeliveryDate;
            et.Remark = purchaseDto.Remark;
            et.AggregateAmount = purchaseDto.ItemDtos.Sum(k => k.CostPrice * k.PurchaseNum);

            var items = purchaseDto.ItemDtos.Select(k => new PurchaseItem
            {
                ItemNo = k.ItemNo,
                ItemName = k.ItemName,
                SuppName = k.SuppName,
                Amount = k.CostPrice * k.PurchaseNum,
                CreateTime = DateTime.Now,
                Creator = request.GetCurrentUser().UserName,
                MerchantGuid = et.MerchantGuid,
                PurchaseNo = et.PurchaseNo,
                PurchaseNum = k.PurchaseNum,
                Remark = k.Remark,
                CostPrice = k.CostPrice
            });

            var packages = purchaseDto.ItemDtos.SelectMany(k => {
                k.Guid = Guid.NewGuid();
                if (k.PackageDtos == null) return null;
                var dtos = mapper.Map<List<PurchasePackage>>(k.PackageDtos);
                dtos.ForEach(p => {
                    p.Amount = p.CostPrice * p.PurchaseNum;
                    p.CreateTime = DateTime.Now;
                    p.Creator = request.GetCurrentUser().UserName;
                    p.MerchantGuid = et.MerchantGuid;
                    p.PurchaseNo = et.PurchaseNo;
                    p.PurchaseItemGuid = k.Guid;
                });
                return dtos;
            });


            await db.PurchaseItems.Where(k => k.PurchaseNo == et.PurchaseNo).ExecuteDeleteAsync();
            await db.PurchasePackages.Where(k => k.PurchaseNo == et.PurchaseNo).ExecuteDeleteAsync();

            await db.PurchaseItems.AddRangeAsync(items);
            await db.PurchasePackages.AddRangeAsync(packages);
            await db.SaveChangesAsync();
            return Results.Ok(et);
        }

        [Authorize]
        public static async Task<IResult> Delete(AppDbContext db, int id, HttpRequest request)
        {
            var et = await db.Purchases.FirstOrDefaultAsync(x => x.Id == id && x.MerchantGuid == request.GetCurrentUser().MerchantGuid);
            if (et == null)
            {
                return Results.BadRequest();
            }
            if (et.IsAudit)
            {
                return Results.BadRequest("已经审核的采购订单信息，不能删除!!");
            }
            var items = await db.PurchaseItems.Where(k => k.PurchaseNo == et.PurchaseNo && k.MerchantGuid == request.GetCurrentUser().MerchantGuid).ToListAsync();
            if (items.Any(k => k.StorageNum > 0))
            {
                return Results.BadRequest("采购订单已经有商品入库，不能删除!!");
            }

            db.PurchaseItems.RemoveRange(items);
            db.Purchases.Remove(et);
            await db.SaveChangesAsync();
            return Results.NoContent();
        }

        [Authorize]
        public static async Task<IResult> Audit(AppDbContext db, int id, HttpRequest request)
        {
            var et = await db.Purchases.FirstOrDefaultAsync(x => x.Id == id && x.MerchantGuid == request.GetCurrentUser().MerchantGuid);
            if (et == null)
            {
                return Results.BadRequest("没有找到采购订单信息!!");
            }
            if (et.IsAudit)
            {
                return Results.BadRequest("采购订单已经审核!!");
            }
            et.IsAudit = true;
            et.AuditDate = DateTime.Now;
            et.AuditUser = request.GetCurrentUser().UserName;
            await db.SaveChangesAsync();
            return Results.Ok(et);
        }

        [Authorize]
        public static async Task<IResult> UnAudit(AppDbContext db, int id, HttpRequest request)
        {
            var et = await db.Purchases.FirstOrDefaultAsync(x => x.Id == id && x.MerchantGuid == request.GetCurrentUser().MerchantGuid);
            if (et == null)
            {
                return Results.BadRequest("没有找到采购订单信息!!");
            }
            var items = await db.PurchaseItems.Where(k => k.PurchaseNo == et.PurchaseNo && k.MerchantGuid == request.GetCurrentUser().MerchantGuid).ToListAsync();
            if (items.Any(k => k.StorageNum > 0))
            {
                return Results.BadRequest("采购订单已经有物品入库，不能退审!!");
            }

            et.IsAudit = false;
            et.AuditDate = null;
            et.AuditUser = string.Empty;
            await db.SaveChangesAsync();
            return Results.Ok(et);
        }

        [Authorize]
        public static async Task<IResult> GetPurchaseProdInfos(AppDbContext db, int id, IMapper mapper, HttpRequest request)
        {
            var et = await db.Purchases.FirstOrDefaultAsync(x => x.Id == id && x.MerchantGuid == request.GetCurrentUser().MerchantGuid);
            if (et == null)
            {
                return Results.BadRequest("没有找到采购订单信息!!");
            }
            var items = await db.PurchaseItems.Where(k => k.PurchaseNo == et.PurchaseNo).ToListAsync();
            var packages = await db.PurchasePackages.Where(k => k.PurchaseNo == et.PurchaseNo).ToListAsync();
            var itemDtos = mapper.Map<IEnumerable<PurchaseItemDto>>(items);
            itemDtos = itemDtos.Select(it => {
                var ips = packages.Where(p=> p.PurchaseItemGuid == it.Guid);
                it.PackageDtos = mapper.Map<List<PurchaseItemDto>>(ips);
                return it;
            });
            return Results.Ok(itemDtos);
        }

        [Authorize]
        public static async Task<IResult> CancelPurchaseItem(AppDbContext db, int id, List<PurchaseItemDto> purchaseItemDtos, HttpRequest request)
        {
            var et = await db.Purchases.FirstOrDefaultAsync(x => x.Id == id && x.MerchantGuid == request.GetCurrentUser().MerchantGuid);
            if (et == null)
            {
                return Results.BadRequest("没有找到采购订单信息!!");
            }

            var items = await db.PurchaseItems.Where(k => k.PurchaseNo == et.PurchaseNo).ToListAsync();
            if ((from k in items
                 from j in purchaseItemDtos
                 where k.Id == j.Id && k.PurchaseNum != j.PurchaseNum
                 select k).Any())
            {
                return Results.BadRequest("采购单的商品数量不匹配!!");
            }

            var packageDtos = purchaseItemDtos.SelectMany(k => k.PackageDtos);
            var packages = await db.PurchasePackages.Where(k => k.PurchaseNo == et.PurchaseNo).ToListAsync();
            if ((from k in packages
                 from j in packageDtos
                 where k.Id == j.Id && k.PurchaseNum != j.PurchaseNum
                 select k).Any())
            {
                return Results.BadRequest("采购单的包件数量不匹配!!");
            }

            var purchaseItemForUpdate = from p in items
                                    join e in purchaseItemDtos
                                    on p.Id equals e.Id
                                    select new { p, e };
            foreach (var up in purchaseItemForUpdate)
            {
                up.p.PurchaseNum -= up.e.CancelNum;
                up.p.CancelNum += up.e.CancelNum;
            }

            var purchasePackageForUpdate = from p in packages
                                           join e in packageDtos
                                            on p.Id equals e.Id
                                            select new { p, e };
            foreach (var up in purchasePackageForUpdate)
            {
                up.p.PurchaseNum -= up.e.CancelNum;
                up.p.CancelNum += up.e.CancelNum;
            }
            db.PurchaseItems.UpdateRange(purchaseItemForUpdate.Select(k=> k.p));
            db.PurchasePackages.UpdateRange(purchasePackageForUpdate.Select(k => k.p));
            await db.SaveChangesAsync();
            return Results.Ok(et);
        }

    }
}
