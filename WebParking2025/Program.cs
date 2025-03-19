using Microsoft.EntityFrameworkCore;
using WebParking2025.Data;
using Microsoft.AspNetCore.Identity;
using WebParking2025.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//Add db context
builder.Services.AddDbContext<ParkingContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));
builder.Services.AddAuthentication()
   .AddGoogle(options =>
   {
       //IConfigurationSection googleAuthNSection =
       //config.GetSection("Authentication:Google");
       options.ClientId = "215654243479-vu2q0gknsgesi4d6aupurrtpce61c7ck.apps.googleusercontent.com";
       options.ClientSecret = "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";
   });
//Identity items
builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ParkingContext>();
    

builder.Services.AddRazorPages();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();
app.Run();
