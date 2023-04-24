using FurnitureERP.Dtos;

namespace FurnitureERP.Controllers
{
    public class SysDictController
    {
        [Authorize]
        public static async Task<IResult> Create(AppDbContext db, SysDictValueDto sysDictValueDto, HttpRequest request, IMapper mapper)
        {
            if (await db.SysDictValues.FirstOrDefaultAsync(x => x.MerchantGuid == request.GetCurrentUser().MerchantGuid 
            && x.DictCode == sysDictValueDto.DictCode && x.DataValue == sysDictValueDto.DataValue) != null)
            {
                return Results.BadRequest("存在相同的数据字典");
            }
            var r = mapper.Map<SysDictValue>(sysDictValueDto);
            r.Creator = request.GetCurrentUser().UserName;
            r.MerchantGuid = request.GetCurrentUser().MerchantGuid;
            db.SysDictValues.Add(r);
            await db.SaveChangesAsync();
            return Results.Ok();
        }

        [Authorize]
        public static async Task<IResult> Get(AppDbContext db, IMapper mapper, HttpRequest request, string dictCode)
        {
            var ets = await db.SysDictValues.Where(x => x.MerchantGuid == request.GetCurrentUser().MerchantGuid && x.DictCode == dictCode).ToListAsync();
            return Results.Ok(mapper.Map<List<SysDictValueDto>>(ets));
        }

        [Authorize]
        public static async Task<IResult> Single(AppDbContext db, int id, IMapper mapper, HttpRequest request)
        {
            var et = await db.SysDictValues.SingleOrDefaultAsync(x => x.Id == id && x.MerchantGuid == request.GetCurrentUser().MerchantGuid);
            return et == null ? Results.NotFound() : Results.Ok(mapper.Map<SysDictValueDto>(et));
        }

        [Authorize]
        public static async Task<IResult> Edit(AppDbContext db, int id, SysDictValueDto sysDictValueDto, HttpRequest request)
        {
            var et = await db.SysDictValues.FirstOrDefaultAsync(x => x.Id == id && x.MerchantGuid == request.GetCurrentUser().MerchantGuid);
            if (et == null)
            {
                return Results.BadRequest("无效的数据");
            }
            if (await db.SysDictValues.FirstOrDefaultAsync(x => x.Id != id && x.MerchantGuid == request.GetCurrentUser().MerchantGuid
            && x.DictCode == sysDictValueDto.DictCode && x.DataValue == sysDictValueDto.DataValue) != null)
            {
                return Results.BadRequest("存在相同的数据字典");
            }
            et.DataValue = sysDictValueDto.DataValue;
            await db.SaveChangesAsync();
            return Results.Ok(et);
        }

        [Authorize]
        public static async Task<IResult> Delete(AppDbContext db, int id, HttpRequest request)
        {
            var et = await db.SysDictValues.FirstOrDefaultAsync(x => x.Id == id && x.MerchantGuid == request.GetCurrentUser().MerchantGuid);
            if (et == null)
            {
                return Results.BadRequest("无效的数据");
            }
            await db.SaveChangesAsync();
            return Results.NoContent();
        }

        [Authorize]
        public static async Task<IResult> GetDicts(AppDbContext db, IMapper mapper)
        {
            var dicts = await db.SysDicts.ToListAsync();
            return dicts == null ? Results.NotFound() : Results.Ok(mapper.Map<SysDictDto>(dicts));
        }
    }
}
