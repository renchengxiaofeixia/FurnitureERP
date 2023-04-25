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
            purchase.SettlementMode = "";

            if (purchaseDto.ItemDtos == null || purchaseDto.ItemDtos.Count < 1)
            {
                return Results.BadRequest();
            }
            var purchaseItems = mapper.Map<List<PurchaseItem>>(purchaseDto.ItemDtos);
            purchaseItems.ForEach(pi =>
            {
                pi.Amount = pi.CostPrice * pi.PurchaseNum;
                pi.CreateTime = DateTime.Now;
                pi.Creator = request.GetCurrentUser().UserName;
                pi.MerchantGuid = purchase.MerchantGuid;
                pi.PurchaseNo = purchase.PurchaseNo;
            });

            await db.PurchaseItems.AddRangeAsync(purchaseItems);

            await db.Purchases.AddAsync(purchase);
            await db.SaveChangesAsync();
            return Results.Created($"/purchase/{purchase.Id}", purchase);
        }

        //public static async Task<IResult> CreateFromTrade(AppDbContext db, CreatePurchaseDto purchaseDto, HttpRequest request, IMapper mapper)
        //{
        //    var purchase = mapper.Map<Purchase>(purchaseDto);
        //    purchase.PurchaseNo = await Util.GetSerialNoAsync(db, request.GetCurrentUser().MerchantGuid, "purchase");
        //    purchase.AggregateAmount = purchaseDto.ItemDtos.Sum(k => k.CostPrice * k.PurchaseNum);
        //    purchase.Creator = request.GetCurrentUser().UserName;
        //    purchase.MerchantGuid = request.GetCurrentUser().MerchantGuid;
        //    purchase.SettlementMode = "";

        //    if (purchaseDto.ItemDtos == null || purchaseDto.ItemDtos.Count < 1)
        //    {
        //        return Results.BadRequest();
        //    }
        //    var purchaseItems = mapper.Map<List<PurchaseItem>>(purchaseDto.ItemDtos);
        //    purchaseItems.ForEach(pi =>
        //    {
        //        pi.Amount = pi.CostPrice * pi.PurchaseNum;
        //        pi.CreateTime = DateTime.Now;
        //        pi.Creator = request.GetCurrentUser().UserName;
        //        pi.MerchantGuid = purchase.MerchantGuid;
        //        pi.PurchaseNo = purchase.PurchaseNo;
        //    });

        //    await db.PurchaseItems.AddRangeAsync(purchaseItems);

        //    await db.Purchases.AddAsync(purchase);
        //    await db.SaveChangesAsync();
        //    return Results.Created($"/purchase/{purchase.Id}", purchase);
        //}

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
            }).ToList();
            var preItems = await db.PurchaseItems.Where(k => k.PurchaseNo == et.PurchaseNo && k.MerchantGuid == et.MerchantGuid).ToListAsync();
            db.PurchaseItems.RemoveRange(preItems);

            await db.PurchaseItems.AddRangeAsync(items);
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
                return Results.BadRequest("采购订单已经有物品入库，不能删除!!");
            }
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
            items = items.OrderByDescending(s => s.Id).ToList();

            return Results.Ok(mapper.Map<List<PurchaseItemDto>>(items));
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

            var purchaseForUpdate = from p in db.PurchaseItems
                                    join e in purchaseItemDtos
                                    on new { p.Id, p.ItemNo } equals new { e.Id, e.ItemNo }
                                    select new { p, e };
            await purchaseForUpdate.ForEachAsync(up =>
            {
                up.p.PurchaseNum -= up.e.CancelNum;
                up.p.CancelNum += up.e.CancelNum;
            });

            return Results.Ok();
        }

    }
}
