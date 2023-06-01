namespace FurnitureERP.Controllers
{
    public class PageColumnController
    {
        [Authorize]
        public static async Task<IResult> Save(AppDbContext db, CreatePageColumnDto pageColumnDto, HttpRequest request, IMapper mapper)
        {
            var et = await db.PageColumns.FirstOrDefaultAsync(x => x.MerchantGuid == request.GetCurrentUser().MerchantGuid 
            && x.PageName == pageColumnDto.PageNamme && x.Creator == request.GetCurrentUser().UserName);

            if (et == null)
            {
                var p = mapper.Map<PageColumn>(pageColumnDto);
                p.PageName = pageColumnDto.PageNamme;
                p.ColumnJson = pageColumnDto.ColumnJson;
                p.Creator = request.GetCurrentUser().UserName;
                p.MerchantGuid = request.GetCurrentUser().MerchantGuid;
                await db.PageColumns.AddAsync(p);
                await db.SaveChangesAsync();
                return Results.Ok(p);
            }
            else
            {
                et.ColumnJson = pageColumnDto.ColumnJson;
                await db.SaveChangesAsync();
                return Results.Ok(et);
            }           
        }

        [Authorize]
        public static async Task<IResult> Single(AppDbContext db, HttpRequest request, IMapper mapper, string pageName)
        {
            var et = await db.PageColumns.SingleOrDefaultAsync(x => x.MerchantGuid == request.GetCurrentUser().MerchantGuid 
            && x.Creator == request.GetCurrentUser().UserName && x.PageName == pageName);

            return et == null ? Results.NotFound() : Results.Ok(mapper.Map<PageColumn>(et));
        }


    }
}
