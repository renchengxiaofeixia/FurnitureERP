using System.ComponentModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using FurnitureERP.Database;
using FurnitureERP.Utils;
using Microsoft.Extensions.Options;
using FurnitureERP.Parameters;


/*
dotnet ef dbcontext scaffold Name=ConnectionStrings:SqlConnection Microsoft.EntityFrameworkCore.SqlServer --data-annotations --context AppDbContext --context-dir Database --output-dir Models --force
 */

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration["ConnectionStrings:SqlConnection"];
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString)); 
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