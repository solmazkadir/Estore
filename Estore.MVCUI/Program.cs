using Estore.Data;
using Estore.Service.Abstract;
using Estore.Service.Concrete;
using Microsoft.AspNetCore.Authentication.Cookies; //oturum iþlemleri için

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DatabaseContext>();

builder.Services.AddTransient(typeof(IService<>), typeof(Service<>)); //Kendi yazdýðýmýz db iþlemlerini yapan servisi .net core da bu þekilde mvc projesine servis olarak tanýtýyoruz ki kullanabilelim.
builder.Services.AddTransient<IProductService, ProductService>(); //Product için yazdýðýmýz özel servisi uygulamaya tanýttýk

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
builder.Services.AddAuthorization();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); //Dikkat! Önce UseAuthentication gelmeli sonra UseAuthorization

app.UseAuthorization();

app.MapControllerRoute(
     name: "admin",
     pattern: "{area:exists}/{controller=Main}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
