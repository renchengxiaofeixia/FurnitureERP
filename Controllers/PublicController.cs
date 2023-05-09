namespace FurnitureERP.Controllers
{
    public class PublicController
    {
        [Authorize]
        public static async Task<IResult> Upload(HttpRequest request)
        {
            if (!request.HasFormContentType)
                return Results.BadRequest();

            var form = await request.ReadFormAsync();
            var fi = form.Files["fi"];
            if (fi is null || fi.Length == 0)
                return Results.BadRequest();

            if (!Path.GetExtension(fi.FileName).EndsWith("jpg") && !Path.GetExtension(fi.FileName).EndsWith("png"))
                return Results.BadRequest("图片只支持jpg和png");

            var svrFn = $"{DateTime.Now.Ticks}{Path.GetExtension(fi.FileName)}";

            var merchantName = request.GetCurrentUser().MerchantName;
            var svrImagePath = Path.Combine(AppContext.BaseDirectory, $@"images\{merchantName}");
            if (!Directory.Exists(svrImagePath)) Directory.CreateDirectory(svrImagePath);
            var svrpath = Path.Combine(svrImagePath, svrFn);
            
            await using var stream = fi.OpenReadStream();
            using var fs = File.Create(svrpath);
            await stream.CopyToAsync(fs);
            string picUrl = $"/images/{merchantName}/{svrFn}";
            return Results.Ok(picUrl);
        }
    }
}
