using HoangNgocKhoa241230762THW.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<QLNhaSachContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("QLNhaSach")));

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
