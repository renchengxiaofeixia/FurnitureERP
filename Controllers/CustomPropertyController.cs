using Azure.Core;
using FurnitureERP.Dtos;

namespace FurnitureERP.Controllers
{
    public class CustomPropertyController
    {
        [Authorize]
        public static async Task<IResult> Create(AppDbContext db, CreateCustomPropertyDto customPropertyDto, HttpRequest request, IMapper mapper)
        {
            var et = await db.CustomProperties.FirstOrDefaultAsync(x => x.ModuleNo == customPropertyDto.ModuleNo && x.MerchantGuid == request.GetCurrentUser().MerchantGuid);
            if (et == null)
            {
                et = mapper.Map<CustomProperty>(customPropertyDto);
            }
            else { 
                et.PropertyConfigJson = customPropertyDto.PropertyConfigJson;
            }

            await db.CustomProperties.AddAsync(et);
            await db.SaveChangesAsync();
            return Results.Created($"/customproperty/{et.Id}", et);
        }

        [Authorize]
        public static async Task<IResult> Single(AppDbContext db, string moduleNo, IMapper mapper, HttpRequest request)
        {
            var et = await db.CustomProperties.SingleOrDefaultAsync(x => x.ModuleNo == moduleNo && x.MerchantGuid == request.GetCurrentUser().MerchantGuid);
            return et == null ? Results.NotFound() : Results.Ok(mapper.Map<CreateCustomPropertyDto>(et));
        }

        [Authorize]
        public static async Task<IResult> GetMods(AppDbContext db, IMapper mapper, HttpRequest request)
        {
            var mods = await db.ErpModules.ToListAsync();
            return mods == null ? Results.NotFound() : Results.Ok(mapper.Map<ErpModuleDto>(mods));
        }
    }
}
