using ContractMonitoringSystem.Common;
using ContractMonitoringSystem.Controllers;
using ContractMonitoringSystem.Hubs;
using ContractMonitoringSystem.Interfaces;
using ContractMonitoringSystem.Models;
using ContractMonitoringSystem.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddMvc();
builder.Services.AddSignalR();
var connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
{
    opt.UseSqlServer(connection);
    opt.EnableSensitiveDataLogging();
},ServiceLifetime.Singleton);

builder.Services.AddScoped<ApplicationDbContext>();
builder.Services.AddScoped(typeof(IBaseRepository<>),typeof(BaseRepository<>));
builder.Services.AddScoped(typeof(IBaseUpload<>),typeof(BaseUpload<>));
builder.Services.AddScoped(typeof(IGetProjectRepository),typeof(GetProjectRepository));
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<BaseController>();
builder.Services.AddScoped<BaseInsert>();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;

});
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.None;
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.MapHub<SiteHub>("/siteHub");
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=SignIn}/{id?}");

app.Run();
