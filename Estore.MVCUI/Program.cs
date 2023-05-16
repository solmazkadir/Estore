using Estore.Data;
using Estore.Service.Abstract;
using Estore.Service.Concrete;
using Microsoft.AspNetCore.Authentication.Cookies; //oturum i�lemleri i�in

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DatabaseContext>();

builder.Services.AddTransient(typeof(IService<>), typeof(Service<>)); //Kendi yazd���m�z db i�lemlerini yapan servisi .net core da bu �ekilde mvc projesine servis olarak tan�t�yoruz ki kullanabilelim.
builder.Services.AddTransient<IProductService, ProductService>(); //Product i�in yazd���m�z �zel servisi uygulamaya tan�tt�k

//Uygulama admin paneli i�in oturum a�ma ayarlar�
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(x =>
{
    x.LoginPath = "/Admin/Login"; //Oturum a�mayan kullan�c�lar�n giri� i�in g�nderilece�i adres
    x.LogoutPath = "/Admin/Logout";
    x.AccessDeniedPath = "/AccessDenied"; //Yetkilendirme ile ekrana eri�im hakk� olmayan kullan�c�lar�n g�nderilece�i sayfa
    x.Cookie.Name = "Administrator"; //Olu�acak cookienin ismi
    x.Cookie.MaxAge = TimeSpan.FromDays(1); //Olu�acak cookie nin �mr�
}); ; //Oturum i�lemleri i�in
//Uygulama admin paneli i�in admin yetkilendirme ayarlar�
builder.Services.AddAuthorization(x =>
{
    x.AddPolicy("AdminPolicy", p => p.RequireClaim("Role","Admin")); //admin paneline giri� yapma yetkisine sahip olanlar� bu kuralla kontrol edece�iz.
    x.AddPolicy("UserPolicy", p => p.RequireClaim("Role","User")); //admin d���nda yekilendirme kullan�rsak bu kural� kullanaca��z(siteye �ye giri�i yapanlar� �n y�zde bir panele eri�tirme gibi)
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); //Dikkat! �nce UseAuthentication gelmeli sonra UseAuthorization

app.UseAuthorization();

app.MapControllerRoute(
     name: "admin",
     pattern: "{area:exists}/{controller=Main}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
