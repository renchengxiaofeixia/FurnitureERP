using Microsoft.AspNetCore.Authorization;
using FurnitureERP.Database;
using FurnitureERP.Dtos;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using FurnitureERP.Models;
using FurnitureERP.Utils;

namespace FurnitureERP.Controllers
{
    public class PurchaseController
    {
        [Authorize]
        public static async Task<IResult> Create(AppDbContext db, CreatePurchaseDto purchaseDto, HttpRequest request, IMapper mapper)
        {
            var purchase = mapper.Map<Purchase>(purchaseDto);
            purchase.PurchaseNo = await Util.GetSerialNoAsync(db,request.GetCurrentUser().MerchantGuid,"purchase");
            purchase.AggregateAmount = purchaseDto.ItemDtos.Sum(k => k.CostPrice * k.PurchNum);
            purchase.Creator = request.GetCurrentUser().UserName;
            purchase.MerchantGuid = request.GetCurrentUser().MerchantGuid;

            var prods = purchaseDto.ItemDtos.Select(k => new PurchaseOrder
            {                
                ItemNo = k.ItemNo,
                ItemName = k.ItemName,
                Amount = k.CostPrice * k.PurchNum,
                CreateTime = DateTime.Now,
                Creator = request.GetCurrentUser().UserName,
                MerchantGuid = purchase.MerchantGuid,
                PurchaseNo = purchase.PurchaseNo,
                PurchNum = k.PurchNum,
                Remark = k.Remark,
                CostPrice = k.CostPrice
            }).ToList();
            await db.PurchaseOrders.AddRangeAsync(prods);
            db.Purchases.Add(purchase);
            await db.SaveChangesAsync();
            return Results.Created($"/purchase/{purchase.Id}", purchase);
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="page">页码</param>
        ///// <param name="size">每页条数</param>
        ///// <param name="wd">关键字</param>
        ///// <returns></returns>
        ////[Authorize]
        //public static async Task<IResult> Get(AppDbContext db, PagingData pageData,IMapper mapper)
        //{
        //    var ets = db.PurchaseOrders.Select(x => x);
        //    if (!string.IsNullOrEmpty(pageData.SearchString))
        //    {
        //        ets = ets.Where(s => s.PurchaseNo.Contains(pageData.SearchString));
        //    }
        //    ets = ets.OrderByDescending(s => s.Id);
        //    var p = await Page<PurchaseOrder>.CreateAsync(ets, pageData.CurrentPage, pageData.PageSize); 
        //    p.Data = mapper.Map<List<PurchaseOrderDto>>(p.Data);
        //    return Results.Ok(p);
        //}

        [Authorize]
        public static async Task<IResult> Single(AppDbContext db, int id,IMapper mapper)
        {
            var et = await db.PurchaseOrders.SingleOrDefaultAsync(x => x.Id == id);
            var purchaseDto = mapper.Map<PurchaseOrderDto>(et);
            return et == null ? Results.NotFound() : Results.Ok(purchaseDto);
        }

        //[Authorize]
        public static async Task<IResult> Edit(AppDbContext db, int id, CreatePurchaseDto purchaseDto, HttpRequest request, IMapper mapper)
        {
            var et = await db.Purchases.FirstOrDefaultAsync(x => x.Id == id);
            if (et == null)
            {
                return Results.BadRequest("没有找到采购订单信息!!");
            }
            et.PurchaseOrderDate = purchaseDto.PurchaseOrderDate;
            //et.SettlementMode = purchaseDto.SettlementMode;
            et.SuppName = purchaseDto.SuppName;
            et.DeliveryDate = purchaseDto.DeliveryDate;
            et.Remark = purchaseDto.Remark;
            et.AggregateAmount = purchaseDto.ItemDtos.Sum(k=>k.CostPrice * k.PurchNum);

            var items = purchaseDto.ItemDtos.Select(k => new PurchaseOrder
            {
                ItemNo = k.ItemNo,
                ItemName = k.ItemName,
                Amount = k.CostPrice * k.PurchNum,
                CreateTime = DateTime.Now,
                Creator = request.GetCurrentUser().UserName,
                MerchantGuid = et.MerchantGuid,
                PurchaseNo = et.PurchaseNo,
                PurchNum = k.PurchNum,
                Remark = k.Remark,
                CostPrice = k.CostPrice
            }).ToList();
            var preItems = await db.PurchaseOrders.Where(k=>k.PurchaseNo == et.PurchaseNo).ToListAsync();
            db.PurchaseOrders.RemoveRange(preItems);

            await db.PurchaseOrders.AddRangeAsync(items);
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
            var items = await db.PurchaseOrders.Where(k => k.PurchaseNo == et.PurchaseNo).ToListAsync();
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
            var et = await db.Purchases.FirstOrDefaultAsync(x => x.Id == id);
            if (et == null)
            {
                return Results.BadRequest("没有找到采购订单信息!!");
            }
            var items = await db.PurchaseOrders.Where(k => k.PurchaseNo == et.PurchaseNo).ToListAsync();
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
        public static async Task<IResult> GetPurchaseProdInfos(AppDbContext db, int id, IMapper mapper)
        {
            var et = await db.Purchases.FirstOrDefaultAsync(x => x.Id == id);
            if (et == null)
            {
                return Results.BadRequest("没有找到采购订单信息!!");
            }
            var items = await db.PurchaseOrders.Where(k => k.PurchaseNo == et.PurchaseNo).ToListAsync();
            items = items.OrderByDescending(s => s.Id).ToList();
            
            return Results.Ok(mapper.Map<List<PurchaseOrderDto>>(items));
        }

        //[Authorize]
        //public static async Task<IResult> CreatePurchasePay(AppDbContext db, CreatePurchasePaymentDto purchasePaymentDto, HttpRequest request, IMapper mapper)
        //{
        //    var purchasePayment = mapper.Map<PurchasePayment>(purchasePaymentDto);
        //    purchasePayment.BillNo = SnowflakeUtil.Generate().ToString();
        //    purchasePayment.Creator = request.HttpContext.User.Identity?.Name;

        //    db.PurchasePayments.Add(purchasePayment);
        //    await db.SaveChangesAsync();
        //    return Results.Created($"/purchase/pay/{purchasePayment.Id}", purchasePayment);
        //}

        //[Authorize]
        //public static async Task<IResult> EditPurchasePay(AppDbContext db, int id, CreatePurchasePaymentDto purchasePaymentDto, HttpRequest request, IMapper mapper)
        //{
        //    var et = await db.PurchasePayments.SingleOrDefaultAsync(x => x.Id == id);
        //    et.Amount = purchasePaymentDto.Amount;
        //    et.PaymentDate = purchasePaymentDto.PaymentDate;
        //    et.PayType = purchasePaymentDto.PayType;
        //    et.Account= purchasePaymentDto.Account;
        //    et.SupplierName = purchasePaymentDto.SupplierName;
        //    et.Remarks = purchasePaymentDto.Remarks;
        //    et.UpdatedTime = DateTime.Now;
        //    et.Updator = request.HttpContext.User.Identity?.Name;
        //    await db.SaveChangesAsync();
        //    return Results.Ok(et);
        //}

        //[Authorize]
        //public static async Task<IResult> SinglePurchasePay(AppDbContext db, int id,IMapper mapper)
        //{
        //    var et = await db.PurchasePayments.SingleOrDefaultAsync(x => x.Id == id);
        //    var purchasePaymentDto = mapper.Map<PurchasePaymentDto>(et);
        //    return et == null ? Results.NotFound() : Results.Ok(purchasePaymentDto);
        //}

        //[Authorize]
        //public static async Task<IResult> GetPurchasePayments(AppDbContext db, int purchaseId, IMapper mapper)
        //{
        //    var et = await db.PurchaseOrders.FirstOrDefaultAsync(x => x.Id == purchaseId);
        //    if (et == null)
        //    {
        //        return Results.BadRequest("没有找到采购订单信息!!");
        //    }
        //    var payments = await db.PurchasePayments.Where(k => k.PurchaseNo == et.PurchaseNo).ToListAsync();
        //    payments = payments.OrderByDescending(s => s.Id).ToList();

        //    return Results.Ok(mapper.Map<List<PurchasePaymentDto>>(payments));
        //}

        //[Authorize]
        //public static async Task<IResult> DeletePurchasePayment(AppDbContext db, int id)
        //{
        //    var et = await db.PurchasePayments.FirstOrDefaultAsync(x => x.Id == id);
        //    if (et == null)
        //    {
        //        return Results.BadRequest();
        //    }
        //    db.PurchasePayments.Remove(et);
        //    await db.SaveChangesAsync();
        //    return Results.NoContent();
        //}

    }
}
