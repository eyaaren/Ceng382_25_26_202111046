var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// ✨ Session ayarını ekle
builder.Services.AddSession(options =>
{
       options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage(); // ✅ Geliştirme ortamında hataları görebilmek için
}

app.UseHttpsRedirection();


app.UseStaticFiles();

// 🌟 Session middleware burada devreye girmeli (Routing'den ÖNCE!)
app.UseSession();

app.UseRouting();

app.UseAuthorization();

// Razor Pages ve Static dosyalar
app.MapRazorPages();

app.Run();
