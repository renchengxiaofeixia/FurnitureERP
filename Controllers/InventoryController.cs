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
    }
}
