
/*
dotnet ef dbcontext scaffold Name=ConnectionStrings:SqlConnection Microsoft.EntityFrameworkCore.SqlServer --data-annotations --context AppDbContext --context-dir Database --output-dir Models --force
 */

using FurnitureERP.Extensions;
using FurnitureERP.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration["ConnectionStrings:SqlConnection"];
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connectionString);
    options.AddInterceptors(new QueryWithNoLockDbCommandInterceptor());
    options.LogTo(Console.WriteLine);
}); 
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNameCaseInsensitive = false;
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.SerializerOptions.WriteIndented = true;
    options.SerializerOptions.Converters.Add(new FurnitureERP.Utils.DateTimeConverter());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region 自定义复杂的策略授权

var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]));
var issuer = builder.Configuration["Jwt:Issuer"];
var audience = builder.Configuration["Jwt:Audience"];
var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature);
var permits = new List<PermissionItem>();
var permitReq = new PermissionRequirement(permits, ClaimTypes.Role, issuer, audience, signingCredentials, expiration: TimeSpan.FromDays(150));
builder.Services.AddAuthorization(options =>
{
    //options.AddPolicy("Permit", policy => policy.Requirements.Add(permitReq));
    var policy = new AuthorizationPolicyBuilder();
    policy.Requirements.Add(permitReq);
    options.DefaultPolicy = policy.Build();
});
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IAuthorizationHandler, PermissionHandler>();
builder.Services.AddSingleton(permitReq);
#endregion

//builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = signingKey
    };
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("cors",
        builder => builder.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod()
    );
});

var app = builder.Build();

#region middleware

if (app.Environment.IsDevelopment())
{
    //异常处理中间件
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    Log.Initiate("运行日志",true);
    app.UseExceptionHandler(exceptionHandlerApp =>
    {
        exceptionHandlerApp.Run(async context =>
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = Text.Plain;
            var exceptionHandlerPathFeature =
                context.Features.Get<IExceptionHandlerPathFeature>();

            await context.Response.WriteAsync(exceptionHandlerPathFeature?.Error.Message);
            //if (exceptionHandlerPathFeature?.Error is FileNotFoundException)
            //{
            //    await context.Response.WriteAsync(" The file was not found.");
            //}
            Log.Exception(exceptionHandlerPathFeature?.Error);
        });
    });
}

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("cors");

var svrImagePath = Path.Combine(AppContext.BaseDirectory, @"images");
if (!Directory.Exists(svrImagePath)) Directory.CreateDirectory(svrImagePath);
var svrExcelPath = Path.Combine(AppContext.BaseDirectory, @"excel");
if (!Directory.Exists(svrExcelPath)) Directory.CreateDirectory(svrExcelPath);
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(svrImagePath),
    RequestPath = new PathString("/images")
});

#endregion

#region routers
FurnitureERP.Routers.Router.Use(app);
#endregion

// start 
app.MapGet("/", () => "Dotnet Minimal API");
app.Run($"https://0.0.0.0:31000");
//app.Run();