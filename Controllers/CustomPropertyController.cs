
namespace FurnitureERP.Controllers
{
    public class CustomPropertyController
    {
        [Authorize]
        public static async Task<IResult> CreatePropConfig(AppDbContext db, CreatePropertyConfigDto propertyConfigDto, HttpRequest request, IMapper mapper)
        {
            var et = await db.PropertyConfigs.FirstOrDefaultAsync(x => x.ModuleNo == propertyConfigDto.ModuleNo && x.MerchantGuid == request.GetCurrentUser().MerchantGuid);
            if (et == null)
            {
                et = mapper.Map<PropertyConfig>(propertyConfigDto);
                et.MerchantGuid = request.GetCurrentUser().MerchantGuid;
                et.Creator = request.GetCurrentUser().UserName;
            }
            else
            {
                et.PropertyConfigJson = JsonSerializer.Serialize(propertyConfigDto.Properties);
            }

            await db.PropertyConfigs.AddAsync(et);
            await db.SaveChangesAsync();
            return Results.Created($"/propconfig/{et.Id}", et);
        }

        [Authorize]
        public static async Task<IResult> SinglePropConfig(AppDbContext db, string moduleNo, IMapper mapper, HttpRequest request)
        {
            var et = await db.PropertyConfigs.SingleOrDefaultAsync(x => x.ModuleNo == moduleNo && x.MerchantGuid == request.GetCurrentUser().MerchantGuid);
            return et == null ? Results.NotFound() : Results.Ok(mapper.Map<PropertyConfigDto>(et));
        }

        [Authorize]
        public static async Task<IResult> GetMods(AppDbContext db, IMapper mapper, HttpRequest request)
        {
            var mods = await db.ErpModules.ToListAsync();
            return mods == null ? Results.NotFound() : Results.Ok(mapper.Map<ErpModuleDto>(mods));
        }
    }
}
