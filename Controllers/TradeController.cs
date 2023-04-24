using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Controllers
{
    public class TradeController
    {

        [Authorize]
        public static async Task<IResult> Create(AppDbContext db, CreateTradeDto tradeDto, HttpRequest request, IMapper mapper)
        {
            var trade = mapper.Map<Trade>(tradeDto);
            trade.Tid = await Util.GetSerialNoAsync(db, request.GetCurrentUser().MerchantGuid, "trade");
            trade.Creator = request.GetCurrentUser().UserName;
            trade.MerchantGuid = request.GetCurrentUser().MerchantGuid;
            if (tradeDto.ItemDtos == null || tradeDto.ItemDtos.Count < 1)
            {
                return Results.BadRequest("订单中必须包含产品信息");
            }
            var purchaseItems = mapper.Map<List<TradeItem>>(tradeDto.ItemDtos);
            purchaseItems.ForEach(pi =>
            {
                pi.Payment = pi.CostPrice * pi.Num;
                pi.CreateTime = DateTime.Now;
                pi.Creator = request.GetCurrentUser().UserName;
                pi.MerchantGuid = trade.MerchantGuid;
                pi.Tid = trade.Tid;
            });

            await db.TradeItems.AddRangeAsync(purchaseItems);

            await db.Trades.AddAsync(trade);
            await db.SaveChangesAsync();
            return Results.Created($"/purchase/{trade.Id}", trade);
        }

        [Authorize]
        public static async Task<IResult> Page(AppDbContext db, IMapper mapper, HttpRequest request
            , string? keyword, DateTime? startCreateTime, DateTime? endCreateTime
            , DateTime? startPayTime, DateTime? endPayTime
            , DateTime? startPrintDate, DateTime? endPrintDate
            , int pageNo, int pageSize)
        {
            IQueryable<Trade> trades = db.Trades.Where(x => x.MerchantGuid == request.GetCurrentUser().MerchantGuid);
            if (!string.IsNullOrEmpty(keyword))
            {
                trades = trades.Where(k => k.Tid.Contains(keyword) || k.ReceiverName.Contains(keyword) || k.BuyerNick.Contains(keyword));
            }
            if (startCreateTime.HasValue)
            {
                trades = trades.Where(k => k.CreateTime >= startCreateTime.Value);
            }
            if (endCreateTime.HasValue)
            {
                trades = trades.Where(k => k.CreateTime <= endCreateTime.Value);
            }
            if (startPayTime.HasValue)
            {
                trades = trades.Where(k => k.PayTime >= startPayTime.Value);
            }
            if (endPayTime.HasValue)
            {
                trades = trades.Where(k => k.PayTime <= endPayTime.Value);
            }
            if (startPrintDate.HasValue)
            {
                trades = trades.Where(k => k.PrintDate >= startPrintDate.Value);
            }
            if (endPrintDate.HasValue)
            {
                trades = trades.Where(k => k.PrintDate <= endPrintDate.Value);
            }
            var page = await Pagination<Trade>.CreateAsync(trades, pageNo, pageSize);
            page.Items = mapper.Map<List<TradeDto>>(page.Items);
            return Results.Ok(page);
        }

        [Authorize]
        public static async Task<IResult> Single(AppDbContext db, long id, IMapper mapper, HttpRequest request)
        {
            var et = await db.Trades.SingleOrDefaultAsync(x => x.Id == id && x.MerchantGuid == request.GetCurrentUser().MerchantGuid);
            var tradeDto = mapper.Map<TradeDto>(et);
            return et == null ? Results.NotFound() : Results.Ok(tradeDto);
        }

        [Authorize]
        public static async Task<IResult> Edit(AppDbContext db, long id, CreateTradeDto tradeDto, HttpRequest request, IMapper mapper)
        {
            var et = await db.Trades.FirstOrDefaultAsync(x => x.Id == id && x.MerchantGuid == request.GetCurrentUser().MerchantGuid);
            if (et == null)
            {
                return Results.BadRequest("没有找到订单信息!!");
            }

            if (tradeDto.ItemDtos == null || tradeDto.ItemDtos.Count < 1)
            {
                return Results.BadRequest();
            }

            et.PayTime = tradeDto.PayTime;
            et.BuyerNick = tradeDto.BuyerNick;
            et.ReceiverName = tradeDto.ReceiverName;
            et.ReceiverMobile = tradeDto.ReceiverMobile;
            et.ReceiverPhone = tradeDto.ReceiverPhone;
            et.ReceiverState = tradeDto.ReceiverState;
            et.ReceiverCity =  tradeDto.ReceiverCity;
            et.ReceiverDistrict = tradeDto.ReceiverDistrict;
            et.ReceiverTown = tradeDto.ReceiverTown;
            et.ReceiverZip = tradeDto.ReceiverZip;
            et.LogisName = tradeDto.LogisName;
            et.Payment = tradeDto.Payment;
            et.IsSelfAdd = tradeDto.IsSelfAdd;
            et.SellerMemo = tradeDto.SellerMemo;
            et.SellerFlag = tradeDto.SellerFlag;
            et.SellerNick = tradeDto.SellerNick;
            et.SellerName = tradeDto.SellerName;
            
            var items = tradeDto.ItemDtos.Select(k => new TradeItem
            {
                ItemNo = k.ItemNo,
                ItemName = k.ItemName,
                StdItemNo = k.StdItemNo,
                Payment = k.Payment,
                CreateTime = DateTime.Now,
                Creator = request.GetCurrentUser().UserName,
                MerchantGuid = et.MerchantGuid,
                Tid = et.Tid,
                Num = k.Num,
                Remark = k.Remark,
                CostPrice = k.CostPrice
            }).ToList();
            var preItems = await db.TradeItems.Where(k => k.Tid == et.Tid && k.MerchantGuid == et.MerchantGuid).ToListAsync();
            db.TradeItems.RemoveRange(preItems);

            await db.TradeItems.AddRangeAsync(items);
            await db.SaveChangesAsync();
            return Results.Ok(et);
        }

        [Authorize]
        public static async Task<IResult> Delete(AppDbContext db, long id, HttpRequest request)
        {
            var et = await db.Trades.FirstOrDefaultAsync(x => x.Id == id && x.MerchantGuid == request.GetCurrentUser().MerchantGuid);
            if (et == null)
            {
                return Results.BadRequest();
            }
            if (et.IsAudit)
            {
                return Results.BadRequest("已经审核的订单，不能删除!!");
            }
            var items = await db.TradeItems.Where(k=>k.Tid== et.Tid).ToListAsync();
            db.Trades.Remove(et);
            db.TradeItems.RemoveRange(items);
            await db.SaveChangesAsync();
            return Results.NoContent();
        }

        [Authorize]
        public static async Task<IResult> Audit(AppDbContext db, long id, HttpRequest request)
        {
            var et = await db.Trades.FirstOrDefaultAsync(x => x.Id == id && x.MerchantGuid == request.GetCurrentUser().MerchantGuid);
            if (et == null)
            {
                return Results.BadRequest("没有找到订单信息!!");
            }
            if (et.IsAudit)
            {
                return Results.BadRequest("订单已经审核!!");
            }
            et.IsAudit = true;
            et.AuditDate = DateTime.Now;
            et.AuditUser = request.GetCurrentUser().UserName;
            await db.SaveChangesAsync();
            return Results.Ok(et);
        }

        [Authorize]
        public static async Task<IResult> UnAudit(AppDbContext db, long id, HttpRequest request)
        {
            var et = await db.Trades.FirstOrDefaultAsync(x => x.Id == id && x.MerchantGuid == request.GetCurrentUser().MerchantGuid);
            if (et == null)
            {
                return Results.BadRequest("没有找到订单信息!!");
            }
            if (et.IsGoodsAudit)
            {
                return Results.BadRequest("订单已经货审，不能退客审!!");
            }
            et.IsAudit = false;
            et.AuditDate = null;
            et.AuditUser = string.Empty;
            await db.SaveChangesAsync();
            return Results.Ok(et);
        }

        [Authorize]
        public static async Task<IResult> GoodsAudit(AppDbContext db, long id, HttpRequest request)
        {
            var et = await db.Trades.FirstOrDefaultAsync(x => x.Id == id && x.MerchantGuid == request.GetCurrentUser().MerchantGuid);
            if (et == null)
            {
                return Results.BadRequest("没有找到订单信息!!");
            }
            if (!et.IsAudit)
            {
                return Results.BadRequest("订单还未客审，不能货审!!");
            }
            if (et.IsGoodsAudit)
            {
                return Results.BadRequest("订单已经货审!!");
            }
            et.IsGoodsAudit = true;
            et.GoodsAuditDate = DateTime.Now;
            et.GoodsAuditUser = request.GetCurrentUser().UserName;
            await db.SaveChangesAsync();
            return Results.Ok(et);
        }

        [Authorize]
        public static async Task<IResult> UnGoodsAudit(AppDbContext db, long id, HttpRequest request)
        {
            var et = await db.Trades.FirstOrDefaultAsync(x => x.Id == id && x.MerchantGuid == request.GetCurrentUser().MerchantGuid);
            if (et == null)
            {
                return Results.BadRequest("没有找到订单信息!!");
            }
            if (et.IsPrint)
            {
                return Results.BadRequest("订单已经打印，不能退货审!!");
            }
            et.IsGoodsAudit = false;
            et.GoodsAuditDate = null;
            et.GoodsAuditUser = string.Empty;
            await db.SaveChangesAsync();
            return Results.Ok(et);
        }

        [Authorize]
        public static async Task<IResult> FinAudit(AppDbContext db, long id, HttpRequest request)
        {
            var et = await db.Trades.FirstOrDefaultAsync(x => x.Id == id && x.MerchantGuid == request.GetCurrentUser().MerchantGuid);
            if (et == null)
            {
                return Results.BadRequest("没有找到订单信息!!");
            }
            if (!et.IsAudit)
            {
                return Results.BadRequest("订单还未客审，不能财审!!");
            }
            if (et.IsFinAudit)
            {
                return Results.BadRequest("订单已经财审!!");
            }
            et.IsFinAudit = true;
            et.FinAuditDate = DateTime.Now;
            et.FinAuditUser = request.GetCurrentUser().UserName;
            await db.SaveChangesAsync();
            return Results.Ok(et);
        }

        [Authorize]
        public static async Task<IResult> UnFinAudit(AppDbContext db, long id, HttpRequest request)
        {
            var et = await db.Trades.FirstOrDefaultAsync(x => x.Id == id && x.MerchantGuid == request.GetCurrentUser().MerchantGuid);
            if (et == null)
            {
                return Results.BadRequest("没有找到订单信息!!");
            }
            if (et.IsPrint)
            {
                return Results.BadRequest("订单已经打印，不能退财审!!");
            }
            et.IsFinAudit = false;
            et.FinAuditDate = null;
            et.FinAuditUser = string.Empty;
            await db.SaveChangesAsync();
            return Results.Ok(et);
        }

        [Authorize]
        public static async Task<IResult> Print(AppDbContext db, List<long> ids, HttpRequest request)
        {
            var ets = await db.Trades.Where(x => ids.All(k=>k == x.Id)  && x.MerchantGuid == request.GetCurrentUser().MerchantGuid).ToListAsync();
            if (ets == null || ets.Count < 1)
            {
                return Results.BadRequest("没有找到订单信息!!");
            }
            //if (!et.IsGoodsAudit)
            //{
            //    return Results.BadRequest("订单还未货审，不能打印!!");
            //}
            //if (!et.IsFinAudit)
            //{
            //    return Results.BadRequest("订单还未财审，不能打印!!");
            //}
            //if (et.IsPrint)
            //{
            //    return Results.BadRequest("订单已经打印!!");
            //}
            //et.IsPrint = true;
            //et.PrintDate = DateTime.Now;
            //et.PrintUser = request.GetCurrentUser().UserName;
            //await db.SaveChangesAsync();
            return Results.Ok(null);
        }

        [Authorize]
        public static async Task<IResult> UnPrint(AppDbContext db, long id, HttpRequest request)
        {
            var et = await db.Trades.FirstOrDefaultAsync(x => x.Id == id && x.MerchantGuid == request.GetCurrentUser().MerchantGuid);
            if (et == null)
            {
                return Results.BadRequest("没有找到订单信息!!");
            }
            if (et.IsSend)
            {
                return Results.BadRequest("订单已经发货，不能退打印!!");
            }
            et.IsPrint = false;
            et.PrintDate = null;
            et.PrintUser = string.Empty;
            await db.SaveChangesAsync();
            return Results.Ok(et);
        }

        [Authorize]
        public static async Task<IResult> Send(AppDbContext db, SendTradeDto sendTradeDto, HttpRequest request)
        {
            var et = await db.Trades.FirstOrDefaultAsync(x => x.Id == sendTradeDto.Id && x.MerchantGuid == request.GetCurrentUser().MerchantGuid);
            if (et == null)
            {
                return Results.BadRequest("没有找到订单信息!!");
            }
            if (!et.IsPrint)
            {
                return Results.BadRequest("订单还未打印，不能发货!!");
            }
            et.IsSend = true;
            et.SendDate = DateTime.Now;
            et.SendUser = request.GetCurrentUser().UserName;
            et.LogisFee = sendTradeDto.LogisFee;
            et.LogisName = sendTradeDto.LogisName;
            et.LogisNo = sendTradeDto.LogisNo;
            et.InstallFee = sendTradeDto.InstallFee;
            et.InstallName = sendTradeDto.InstallName;
            await db.SaveChangesAsync();
            return Results.Ok(et);
        }

        [Authorize]
        public static async Task<IResult> GetTradeProdInfos(AppDbContext db, long id, IMapper mapper, HttpRequest request)
        {
            var et = await db.Trades.FirstOrDefaultAsync(x => x.Id == id && x.MerchantGuid == request.GetCurrentUser().MerchantGuid);
            if (et == null)
            {
                return Results.BadRequest("没有找到订单信息!!");
            }
            var items = await db.TradeItems.Where(k => k.Tid == et.Tid).ToListAsync();
            items = items.OrderByDescending(s => s.Id).ToList();

            return Results.Ok(mapper.Map<List<TradeItemDto>>(items));
        }

    }
}
