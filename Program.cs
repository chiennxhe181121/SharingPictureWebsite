using Microsoft.EntityFrameworkCore;
using SharingPictureWebsite.Data;
using SharingPictureWebsite.Repositories;
using SharingPictureWebsite.Repositories.Interfaces;
using SharingPictureWebsite.Services;
using SharingPictureWebsite.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// ================= ADD SERVICES =================
builder.Services.AddControllersWithViews();

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ================= DI: REPOSITORY =================
builder.Services.AddScoped<IPictureRepository, PictureRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IAlbumRepository, AlbumRepository>();
builder.Services.AddScoped<IAlbumPictureRepository, AlbumPictureRepository>();

// ================= DI: SERVICE =================
builder.Services.AddScoped<IPictureService, PictureService>();

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