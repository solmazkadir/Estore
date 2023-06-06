using Estore.Data;
using Estore.Service.Abstract;
using Estore.Service.Concrete;
using Microsoft.AspNetCore.Authentication.Cookies; //oturum iþlemleri için

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession(); // uygulamada session kullanabilmek için

builder.Services.AddDbContext<DatabaseContext>();

builder.Services.AddTransient(typeof(IService<>), typeof(Service<>)); //Kendi yazdýðýmýz db iþlemlerini yapan servisi .net core da bu þekilde mvc projesine servis olarak tanýtýyoruz ki kullanabilelim.
builder.Services.AddTransient<IProductService, ProductService>(); //Product için yazdýðýmýz özel servisi uygulamaya tanýttýk. AddTransient yöntemiyle servis eklediðimizde sistem uygulamayý çalýþtýrdýðýnda hazýrda ayný nesne varsa o kullanýlýr yoksa yeni bir nesne oluþturup kullanýma sunulur

//builder.Services.AddSingleton<IProductService, ProductService>(); //AddSingleton yöntemiyle servis eklediðimizde sistem uygulamayý çalýþtýrýrken bu nesneden 1 tane üretir ve her istekte ayný nesne gönderilir. Performans olarak diðerlerinden iyi yöntemdir
//builder.Services.AddScoped<IProductService, ProductService>(); //AddScoped yöntemiyle servis eklediðimizde sistem uygulamayý çalýþtýrýrken bu nesneye gelen her istek için ayrý nesneler üretip bunu kullanýma sunar. Ýçeriðin çok dinamik bir þekilde sürekli deðiþtiði projelerde kullanýlabilir. Döviz altýn fiyatý gibi anlýk deðiþimlerin olduðu projelerde mesela...

//Uygulama admin paneli için oturum açma ayarlarý
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(x =>
{
    x.LoginPath = "/Admin/Login"; //Oturum açmayan kullanýcýlarýn giriþ için gönderileceði adres
    x.LogoutPath = "/Admin/Logout";
    x.AccessDeniedPath = "/AccessDenied"; //Yetkilendirme ile ekrana eriþim hakký olmayan kullanýcýlarýn gönderileceði sayfa
    x.Cookie.Name = "Administrator"; //Oluþacak cookienin ismi
    x.Cookie.MaxAge = TimeSpan.FromDays(1); //Oluþacak cookie nin ömrü
}); ; //Oturum iþlemleri için
//Uygulama admin paneli için admin yetkilendirme ayarlarý
builder.Services.AddAuthorization(x =>
{
    x.AddPolicy("AdminPolicy", p => p.RequireClaim("Role","Admin")); //admin paneline giriþ yapma yetkisine sahip olanlarý bu kuralla kontrol edeceðiz.
    x.AddPolicy("UserPolicy", p => p.RequireClaim("Role","User")); //admin dýþýnda yekilendirme kullanýrsak bu kuralý kullanacaðýz(siteye üye giriþi yapanlarý ön yüzde bir panele eriþtirme gibi)
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseSession();//session için

app.UseAuthentication(); //Dikkat! Önce UseAuthentication gelmeli sonra UseAuthorization

app.UseAuthorization();

app.MapControllerRoute(
     name: "admin",
     pattern: "{area:exists}/{controller=Main}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
