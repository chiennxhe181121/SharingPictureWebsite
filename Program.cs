using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SharingPictureWebsite.Data;
using SharingPictureWebsite.Repositories;
using SharingPictureWebsite.Repositories.Interfaces;
using SharingPictureWebsite.Services;
using SharingPictureWebsite.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// ================= ADD SERVICES =================
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/AccessDenied"; // tạo view AccessDenied
        options.ExpireTimeSpan = TimeSpan.FromHours(4);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ================= DI: REPOSITORY =================
builder.Services.AddScoped<IPictureRepository, PictureRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IAlbumRepository, AlbumRepository>();
builder.Services.AddScoped<IAlbumPictureRepository, AlbumPictureRepository>();
builder.Services.AddScoped<IMemberRepository, MemberRepository>();

// ================= DI: SERVICE =================
builder.Services.AddScoped<IPictureService, PictureService>();
builder.Services.AddScoped<IMemberService, MemberService>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// ================= MIDDLEWARE =================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // 🔥 QUAN TRỌNG cho upload ảnh
app.UseRouting();

// ❌ Session vẫn có thể giữ, nhưng auth cookie bắt buộc phải có
app.UseSession();

app.UseAuthentication();  // phải trước UseAuthorization
app.UseAuthorization();

// ================= ROUTING =================
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

// ================= MIGRATION + SEED =================
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // ❌ KHÔNG dùng EnsureCreated nữa
    context.Database.Migrate(); // 🔥 chuẩn Code First

    SeedData.Initialize(context);
}

app.Run();