using AnimeShop.Models;
using AnimeShop.Models.Validations;
using AnimeShop.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();

builder.Services.AddWebOptimizer(options =>
{
    options.CompileScssFiles();
});

builder.Services.AddDbContext<ShopDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});

builder.Services.AddScoped<ShopDBContext>();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddTransient<IValidator, ProductValidation>();
builder.Services.AddTransient<IValidator, CategoriesValidation>();
builder.Services.AddTransient<IValidator, UserValidation>();



builder.Services.AddTransient<IShopModelService, ShopModelService>();
builder.Services.AddTransient<IShopService, ShopService>();
builder.Services.AddTransient<ILoginService, LoginService>();
builder.Services.AddTransient<IMailService, MailService>();

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
app.UseWebOptimizer();
app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Start}/{action=Start}/{id?}"
    );


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
