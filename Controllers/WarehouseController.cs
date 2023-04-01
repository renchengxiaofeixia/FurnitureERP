
using Azure.Core;

namespace FurnitureERP.Controllers
{
    public class WarehouseController
    {
        [Authorize]
        public static async Task<IResult> Create(AppDbContext db, CreateWarehouseDto WarehouseDto, HttpRequest request, IMapper mapper)
        {
            var warehouse = mapper.Map<Warehouse>(WarehouseDto);
            warehouse.Creator = request.GetCurrentUser().UserName;
            warehouse.MerchantGuid = request.GetCurrentUser().MerchantGuid;
            db.Warehouses.Add(warehouse);
            await db.SaveChangesAsync();
            return Results.Created($"/warehouse/{warehouse.Id}", warehouse);
        }

        [Authorize]
        public static async Task<IResult> Get(AppDbContext db, IMapper mapper, HttpRequest request)
        {
            var ets =await db.Warehouses.Where(k=>k.MerchantGuid == request.GetCurrentUser().MerchantGuid).ToListAsync();
            return Results.Ok(mapper.Map<List<WarehouseDto>>(ets));
        }

        [Authorize]
        public static async Task<IResult> Single(AppDbContext db, int id, IMapper mapper)
        {
            var et = await db.Warehouses.SingleOrDefaultAsync(x => x.Id == id);
            return et == null ? Results.NotFound() : Results.Ok(mapper.Map<WarehouseDto>(et));
        }

        [Authorize]
        public static async Task<IResult> Edit(AppDbContext db, int id, CreateWarehouseDto warehouseDto, HttpRequest request, IMapper mapper)
        {
            var et = await db.Warehouses.FirstOrDefaultAsync(x => x.Id == id);
            if (et == null)
            {
                return Results.BadRequest();
            }
            et.WarehouseName = warehouseDto.WarehouseName;
            await db.SaveChangesAsync();
            return Results.Ok(et);
        }

        [Authorize]
        public static async Task<IResult> Delete(AppDbContext db, int id, HttpRequest request)
        {
            var et = await db.Warehouses.FirstOrDefaultAsync(x => x.Id == id);
            if (et == null)
            {
                return Results.BadRequest();
            }
            db.Warehouses.Remove(et);
            await db.SaveChangesAsync();
            return Results.NoContent();
        }
    }
}
