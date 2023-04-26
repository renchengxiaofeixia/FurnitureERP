
using Azure.Core;
using FurnitureERP.Enums;

namespace FurnitureERP.Controllers
{
    public class SysParamController
    {
        [Authorize]
        public static async Task<IResult> Save(AppDbContext db, string name ,string value, HttpRequest request)
        {
            var param = db.SysParams.FirstOrDefault(k => k.MerchantGuid == request.GetCurrentUser().MerchantGuid && k.ParamName == name);
            if (param == null)
            {
                param = new SysParam();
                param.MerchantGuid = request.GetCurrentUser().MerchantGuid;
                param.CreateTime = DateTime.Now;
            }
            param.ParamName = name;
            param.ParamValue = value;
            Params.SaveParam(param);
            await db.AddAsync(param);
            await db.SaveChangesAsync();
            return Results.Created($"/params/{param.Id}", param);
        }

        [Authorize]
        public static async Task<IResult> Get(AppDbContext db, HttpRequest request)
        {
            var ets = await db.SysParams.Where(x => x.MerchantGuid == request.GetCurrentUser().MerchantGuid).ToListAsync();
            return Results.Ok(ets.Select(k=>new {
                Name = k.ParamName,
                Value = k.ParamValue,
            }));
        }

    }
}
