using AutoMapper;
using Azure.Core;
using FurnitureERP.Enums;
using FurnitureERP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Cryptography;

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
            return Results.Created($"/trade/{trade.Id}", trade);
        }

        [Authorize]
        public static async Task<IResult> Page(AppDbContext db, IMapper mapper, HttpRequest request
            , string? keyword, DateTime? startCreateTime, DateTime? endCreateTime
            , DateTime? startPayTime, DateTime? endPayTime
            , DateTime? startPrintDate, DateTime? endPrintDate
            , string? receiverName
            , string? receiverMobile
            , string? logisName
            , string? logisNo
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
            if (!string.IsNullOrEmpty(receiverName))
            {
                trades = trades.Where(k => k.ReceiverName.Contains(receiverName));
            }
            if (!string.IsNullOrEmpty(receiverMobile))
            {
                trades = trades.Where(k => k.ReceiverMobile.Contains(receiverMobile));
            }
            if (!string.IsNullOrEmpty(logisName))
            {
                trades = trades.Where(k => k.LogisName.Contains(logisName));
            }
            if (!string.IsNullOrEmpty(logisNo))
            {
                trades = trades.Where(k => k.LogisNo.Contains(logisNo));
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
            //删除手动分配库存记录
            var tradeItemMatchInventorys = await db.TradeItemMatchInventories.Where(k => k.Tid == et.Tid).ToListAsync();
            db.TradeItemMatchInventories.RemoveRange(tradeItemMatchInventorys);

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
        public static async Task<IResult> Print(AppDbContext db, List<long> ids, HttpRequest request,IMapper mapper)
        {
            var trades = await db.Trades.Where(x => ids.All(k=>k == x.Id)  && x.MerchantGuid == request.GetCurrentUser().MerchantGuid).ToListAsync();
            if (trades == null || trades.Count < 1)
            {
                return Results.BadRequest("没有找到订单信息!!");
            }
            if (trades.Any(et => !et.IsGoodsAudit))
            {
                return Results.BadRequest("选择的订单中存在未货审订单，请刷新数据后重试!!");
            }
            if (trades.Any(et => !et.IsFinAudit))
            {
                return Results.BadRequest("选择的订单中存在未财审订单，请刷新数据后重试!!");
            }
            if (trades.Any(et => et.IsPrint))
            {
                return Results.BadRequest("选择的订单中存在已经打印的订单，请刷新数据后重试!!");
            }
            try
            {
                //if (InventoryReceiveEnum.Scan)
                //{                    
                //    if (InventoryDimensionEnum.Item)
                //    {
                //        MinusItemInventory(trades, db);
                //    }
                //    if (InventoryDimensionEnum.Package)
                //    {
                //        MinusPackageInventory(trades, db);
                //    }
                //}

                return await MinusItemInventory(trades, db, mapper, request);
                
            }
            catch {
                return Results.Ok("扣减库存出现异常，请刷新数据重新打印");
            }
        }

        private static async Task<IResult> MinusPackageInventory(List<Trade> trades, AppDbContext db, IMapper mapper, HttpRequest request)
        {
            var tids = trades.Select(k => k.Tid);
            var tradeItemQueryable = db.TradeItems.Where(k => tids.All(tid => tid == k.Tid));
            var inventoryQueryable = from invt in db.Inventories
                                     join it in tradeItemQueryable
                                     on new { invt.ItemNo, invt.WareName } equals new { it.ItemNo, it.WareName }
                                     where invt.MerchantGuid == request.GetCurrentUser().MerchantGuid && invt.Quantity > 0
                                     select invt;
            //单品
            var items = await tradeItemQueryable.Where(k => !k.IsCom).OrderBy(k => k.ItemNo).ThenBy(k => k.Num).ToListAsync();
            //拆分组合商品
            var subItems = await (from comit in tradeItemQueryable
                                  join subit in db.SubItems
                                  on comit.ItemNo equals subit.ItemNo
                                  join it in db.Items
                                  on subit.SubItemNo equals it.ItemNo
                                  select new TradeItemDto()
                                  {
                                      Guid = comit.Guid,
                                      ItemNo = it.ItemNo,
                                      ItemName = it.ItemName,
                                      Num = comit.Num * subit.Num,
                                      WareName = comit.WareName,
                                      Tid = comit.Tid,
                                      Creator = comit.Creator,
                                  }).ToListAsync();
            var itemDtos = mapper.Map<List<TradeItemDto>>(items);
            itemDtos.AddRange(subItems);
            //按商品数量 少->多 排序
            itemDtos = itemDtos.OrderBy(k => k.ItemNo).ThenBy(k => k.Num).ToList();


            /**
             * 库存数  少->多 排序 会优先清空更多的库位
             * 库存数  多->少 排序 会提高分拣效率更多的货可以从同一个库位出货
             * 还可以按入库时间排序，可以符合仓库先进先出原则
             * 清空库位 并且 拣货效率同时优先 ，三个月前的库存优先出货 (暂时只是想法)
            **/
            //按库存商品数量 多->少 排序
            var inventories = await inventoryQueryable.OrderBy(k => k.ItemNo).ThenByDescending(k => k.Quantity).ToListAsync();

            //检查库存数
            var summaryItems = itemDtos.GroupBy(k => new { k.ItemNo, k.WareName })
                .Select(g => new { g.Key.WareName, g.Key.ItemNo, Num = g.Sum(k => k.Num) });
            var summaryInvts = inventories.GroupBy(k => new { k.ItemNo, k.WareName })
                .Select(g => new { g.Key.WareName, g.Key.ItemNo, Quantity = g.Sum(k => k.Quantity) });
            if ((from invt in summaryInvts
                 from it in summaryItems
                 where invt.Quantity < it.Num
                 select it.ItemNo).Any())
            {
                return Results.BadRequest("商品库存不足，不能打印!!!");
            }

            await db.Database.BeginTransactionAsync();
            var tradePickInventoryLogs = new List<TradePickInventoryLog>();
            foreach (var it in itemDtos)
            {
                var invts = inventories.Where(invt => invt.ItemNo == it.ItemNo && invt.WareName == it.WareName);
                foreach (var invt in invts)
                {
                    if (invt.Quantity >= it.Num)
                    {
                        tradePickInventoryLogs.Add(CreatePickLog(invt, it, it.Num));
                        invt.Quantity -= it.Num;
                        it.Num = 0;
                        break;
                    }
                    else
                    {
                        tradePickInventoryLogs.Add(CreatePickLog(invt, it, invt.Quantity));
                        invt.Quantity = 0;
                        it.Num -= invt.Quantity;
                    }
                }
            }
            if (itemDtos.Any(k => k.Num > 0))
            {
                await db.Database.RollbackTransactionAsync();
                return Results.BadRequest("扣减库存出现异常，请刷新数据重新打印");
            }

            trades.ForEach(k =>
            {
                k.IsPrint = true;
                k.PrintDate = DateTime.Now;
                k.PrintUser = request.GetCurrentUser().UserName;
            });
            await db.TradePickInventoryLogs.AddRangeAsync(tradePickInventoryLogs);
            await db.SaveChangesAsync();
            await db.Database.CommitTransactionAsync();

            return Results.Ok(trades);
        }


        private static async Task<IResult> MinusItemInventory(List<Trade> trades, AppDbContext db,IMapper mapper,HttpRequest request)
        {
            var tids = trades.Select(k => k.Tid);
            var tradeItemQueryable = db.TradeItems.Where(k => tids.All(tid => tid == k.Tid));
            var inventoryQueryable = from invt in db.Inventories
                                     join it in tradeItemQueryable
                                     on new { invt.ItemNo, invt.WareName } equals new { it.ItemNo, it.WareName }
                                     where invt.MerchantGuid == request.GetCurrentUser().MerchantGuid && invt.Quantity > 0
                                     select invt;
            //单品
            var items = await tradeItemQueryable.Where(k => !k.IsCom).OrderBy(k => k.ItemNo).ThenBy(k => k.Num).ToListAsync(); 
            var itemDtos = mapper.Map<List<TradeItemDto>>(items); 
            //拆分组合商品
            var subItems = await (from comit in tradeItemQueryable
                                  join subit in db.SubItems
                                  on comit.ItemNo equals subit.ItemNo
                                  join it in db.Items
                                  on subit.SubItemNo equals it.ItemNo
                                  select new TradeItemDto() {
                                      Guid = comit.Guid,
                                      ItemNo = it.ItemNo,
                                      ItemName = it.ItemName,
                                      Num = comit.Num * subit.Num,
                                      WareName = comit.WareName,
                                      Tid = comit.Tid,
                                      Creator = comit.Creator,
                                  }).ToListAsync(); 
            itemDtos.AddRange(subItems);
            //按商品数量 少->多 排序
            itemDtos = itemDtos.OrderBy(k => k.ItemNo).ThenBy(k => k.Num).ToList();

            /**
             * 库存数  少->多 排序 会优先清空更多的库位
             * 库存数  多->少 排序 会提高分拣效率更多的货可以从同一个库位出货
             * 还可以按入库时间排序，可以符合仓库先进先出原则
             * 清空库位 并且 拣货效率同时优先 ，三个月前的库存优先出货 (暂时只是想法)
            **/
            //按库存商品数量 多->少 排序
            var inventories = await inventoryQueryable.OrderBy(k => k.ItemNo).ThenByDescending(k => k.Quantity).ToListAsync();

            //检查库存数
            var summaryItems = itemDtos.GroupBy(k => new { k.ItemNo, k.WareName })
                .Select(g => new { g.Key.WareName, g.Key.ItemNo, Num = g.Sum(k => k.Num) });
            var summaryInvts = inventories.GroupBy(k => new { k.ItemNo, k.WareName })
                .Select(g => new { g.Key.WareName, g.Key.ItemNo, Quantity = g.Sum(k => k.Quantity) });
            if ((from invt in summaryInvts
                 from it in summaryItems
                 where invt.Quantity < it.Num
                 select it.ItemNo).Any())
            {
                return Results.BadRequest("商品库存不足，不能打印!!!");
            }

            await db.Database.BeginTransactionAsync();
            var tradePickInventoryLogs = new List<TradePickInventoryLog>();
            //需要扣减的库存数据
            var updateInventories = new Dictionary<long,Inventory>();
            foreach (var it in itemDtos)
            {
                var invts = inventories.Where(invt => invt.ItemNo == it.ItemNo && invt.WareName == it.WareName);
                foreach (var invt in invts)
                {
                    if (invt.Quantity >= it.Num)
                    {
                        tradePickInventoryLogs.Add(CreatePickLog(invt, it, it.Num));
                        invt.Quantity -= it.Num;
                        it.Num = 0;
                        break;
                    }
                    else
                    {
                        tradePickInventoryLogs.Add(CreatePickLog(invt, it, invt.Quantity));
                        invt.Quantity = 0;
                        it.Num -= invt.Quantity;
                    }
                    updateInventories[invt.Id] = invt;
                }
            }
            if (itemDtos.Any(k => k.Num > 0))
            {
                await db.Database.RollbackTransactionAsync();
                return Results.BadRequest("扣减库存出现异常，请刷新数据重新打印");
            }

            trades.ForEach(k =>
            {
                k.IsPrint = true;
                k.PrintDate = DateTime.Now;
                k.PrintUser = request.GetCurrentUser().UserName;
            });
            await db.TradePickInventoryLogs.AddRangeAsync(tradePickInventoryLogs);
            db.UpdateRange(updateInventories.Values);
            await db.SaveChangesAsync();
            await db.Database.CommitTransactionAsync();

            return Results.Ok(trades);
        }

        private static TradePickInventoryLog CreatePickLog(Inventory invt, TradeItemDto it,int minusQuantity)
        {
            return new TradePickInventoryLog {
                Guid = Guid.NewGuid(),
                AreaName = invt.AreaName,
                WareName = invt.WareName,
                LocationName = invt.LocationName,
                CreateTime = DateTime.Now,
                CostPrice = invt.CostPrice,
                InventoryGuid = invt.Guid,
                InventoryId = invt.Id,
                InventoryQuantity = invt.Quantity,
                ItemNo = invt.ItemNo,
                MinusQuantity = minusQuantity,
                TradeItemGuid = it.Guid,
                PrintSession = Guid.NewGuid(),
                PurchaseNo = invt.PurchaseNo,
                StorageNo = invt.StorageNo,
                Remark = invt.Remark,
                SuppName = invt.SuppName,
                Tid = it.Tid
            };
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

            var tradePickInventoryLogs = await db.TradePickInventoryLogs.Where(k=>k.Tid == et.Tid).ToListAsync();

            var inventoryForUpdate = from p in db.Inventories
                                    join e in db.TradePickInventoryLogs
                                    on p.Id equals e.InventoryId
                                    where e.Tid == et.Tid
                                    select new { p, e };
            await inventoryForUpdate.ForEachAsync(up => up.p.Quantity += up.e.MinusQuantity);
            et.IsPrint = false;
            et.PrintDate = null;
            et.PrintUser = string.Empty;
            db.TradePickInventoryLogs.RemoveRange(tradePickInventoryLogs);
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
        public static async Task<IResult> CreateMatchInventory(AppDbContext db,string tid,[FromBody] List<CreateTradeItemMatchInventoryDto> tradeItemMatchInventoryDtos, IMapper mapper, HttpRequest request)
        {
            var tradeItemMatchInventorys = await db.TradeItemMatchInventories.Where(k=>k.Tid == tid).ToListAsync();
            var addTradeItemMatchInventorys = mapper.Map<List<TradeItemMatchInventory>>(tradeItemMatchInventoryDtos);
            addTradeItemMatchInventorys.ForEach(k => {
                k.AllocInventoryDate = DateTime.Now;
                k.AllocInventoryUser = request.GetCurrentUser().UserName;
                k.CreateTime = DateTime.Now;
                k.Creator = request.GetCurrentUser().UserName;
                k.MerchantGuid = request.GetCurrentUser().MerchantGuid;
            });
            try
            {
                await db.Database.BeginTransactionAsync();
                db.TradeItemMatchInventories.RemoveRange(tradeItemMatchInventorys);
                await db.TradeItemMatchInventories.AddRangeAsync(addTradeItemMatchInventorys);
                await db.SaveChangesAsync();
                await db.Database.CommitTransactionAsync();
            }
            catch { }
           return Results.Ok(addTradeItemMatchInventorys);
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

        [Authorize]
        public static async Task<IResult> CreateTradePay(AppDbContext db, CreateTradePayDto tradePayDto, HttpRequest request, IMapper mapper)
        {
            var tradePay = mapper.Map<TradePay>(tradePayDto);
            tradePay.CreateTime = DateTime.Now;
            tradePay.Creator = request.GetCurrentUser().UserName;
            tradePay.MerchantGuid = request.GetCurrentUser().MerchantGuid;

            db.TradePays.Add(tradePay);
            await db.SaveChangesAsync();
            return Results.Created($"/trade/pay/{tradePay.Id}", tradePay);
        }

        [Authorize]
        public static async Task<IResult> EditTradePay(AppDbContext db, long id, CreateTradePayDto tradePayDto)
        {
            var et = await db.TradePays.SingleOrDefaultAsync(x => x.Id == id);
            if (et == null)
            {
                return Results.BadRequest("没有找到订单支付信息!!");
            }
            et.Tid = tradePayDto.Tid;
            et.Remark = tradePayDto.Remark;
            et.Payment = tradePayDto.Payment;
            et.PayTime = tradePayDto.PayTime;
            et.PayWay = tradePayDto.PayWay;
            await db.SaveChangesAsync();
            return Results.Ok(et);
        }

        [Authorize]
        public static async Task<IResult> SingleTradePay(AppDbContext db, long id, IMapper mapper)
        {
            var et = await db.TradePays.SingleOrDefaultAsync(x => x.Id == id);
            var tradePayDto = mapper.Map<TradePayDto>(et);
            return et == null ? Results.NotFound() : Results.Ok(tradePayDto);
        }

        [Authorize]
        public static async Task<IResult> GetTradePays(AppDbContext db, string tid, IMapper mapper)
        {
            var payments = await db.TradePays.Where(k => k.Tid == tid).ToListAsync();
            payments = payments.OrderByDescending(s => s.Id).ToList();
            return Results.Ok(mapper.Map<List<TradePayDto>>(payments));
        }

        [Authorize]
        public static async Task<IResult> DeleteTradePay(AppDbContext db, long id)
        {
            var et = await db.TradePays.FirstOrDefaultAsync(x => x.Id == id);
            if (et == null)
            {
                return Results.BadRequest();
            }
            db.TradePays.Remove(et);
            await db.SaveChangesAsync();
            return Results.NoContent();
        }

    }
}
