
using FourLeafCloverShoe.Data;
using FourLeafCloverShoe.Helper;
using FourLeafCloverShoe.IServices;
using FourLeafCloverShoe.Services;
using FourLeafCloverShoe.Share.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add services to the container.

builder.Services.AddDbContext<MyDbContext>(options =>
             options.UseSqlServer(
                 builder.Configuration.GetConnectionString("DefaultConnection")));





// add identity
builder.Services.AddIdentity<User, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<MyDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();

// add dependency injection
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICartItemService, CartItemService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderItemService, OrderItemService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductDetailService, ProductDetailService>();
builder.Services.AddScoped<IColorsService, ColorsService>();
builder.Services.AddScoped<IProductImageService, ProductImageService>();
builder.Services.AddScoped<IRanksService, RanksService>();
builder.Services.AddScoped<IRateService, RateService>();
builder.Services.AddScoped<ISizeService, SizeService>();
builder.Services.AddScoped<IUserVoucherService, UserVoucherService>();
builder.Services.AddScoped<IVoucherService, VoucherService>();
builder.Services.AddScoped<IMaterialService, MaterialService>();



// add authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Create", policy => policy.RequireClaim("Create", "True"));
    options.AddPolicy("Edit", policy => policy.RequireClaim("Edit", "True"));
    options.AddPolicy("Delete", policy => policy.RequireClaim("Delete", "True"));
});

//add authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})

  .AddCookie( options=>
    {
        options.LoginPath = "/Identity/Acount/Login";
        options.LogoutPath = "/Identity/Acount/Logout";
        options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
        
    })
  .AddGoogle(options =>
  {     // Đọc thông tin Authentication:Google
      IConfigurationSection googleAuthNSection = builder.Configuration.GetSection("Authentication:Google");
      // Thiết lập ClientID và ClientSecret để truy cập API google
      options.ClientId = googleAuthNSection["ClientId"];
      options.ClientSecret = googleAuthNSection["ClientSecret"];
      // Cấu hình Url callback lại từ Google (không thiết lập thì mặc định là /signin-google)
      options.CallbackPath = "/signin-google";
  })
//.AddFacebook(options =>
//  {     // Đọc thông tin Authentication:facebook
//      IConfigurationSection facebookAuthNSection = builder.Configuration.GetSection("Authentication:Facebook");
//      // Thiết lập ClientID và ClientSecret để truy cập API google
//      options.ClientId = facebookAuthNSection["ClientId"];
//      options.ClientSecret = facebookAuthNSection["ClientSecret"];
//      // Cấu hình Url callback lại từ Google (không thiết lập thì mặc định là /signin-google)
//  })

;

// add razor page
builder.Services.AddRazorPages();
//add AddSignalR
builder.Services.AddSignalR();
// add session

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(7);
});

var app = builder.Build();
app.UseCors(builder => builder
    .WithOrigins("https://localhost:7116")
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials());
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

// use session
app.UseSession();

//use authen + author
app.UseAuthentication();
app.UseAuthorization();

// add endpoins
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
    endpoints.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

    endpoints.MapHub<Hubs>("/Hubs");
});
app.MapRazorPages();
IWebHostEnvironment env = app.Environment;
Rotativa.AspNetCore.RotativaConfiguration.Setup(env.WebRootPath, "../Rotativa/Windows");
app.Run();
