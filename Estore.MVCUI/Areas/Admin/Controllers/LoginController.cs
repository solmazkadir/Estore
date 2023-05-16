using Estore.Core.Entities;
using Estore.MVCUI.Models;
using Estore.Service.Abstract;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Estore.MVCUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoginController : Controller
    {
        private readonly IService<AppUser> _service;

        public LoginController(IService<AppUser> service)
        {
            _service = service;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(); //Sistemden çıkış yap
            return Redirect("/Admin/Login"); //Tekrardan giriş sayfasına yönlendir
        }
        [HttpPost]
        public async Task<IActionResult> Index(AdminLoginViewModel admin)
        {
            try
            {
                var kullanici = await _service.GetAsync(k => k.IsActive && k.Email == admin.Email && k.Password == admin.Password);
                if (kullanici != null)
                {
                    var kullaniciYetkileri = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, kullanici.Email),
                        new Claim("Role", kullanici.IsAdmin ?"Admin" : "User"),
                        new Claim("UserGuid", kullanici.UserGuid.ToString())

                    };
                    var kullaniciKimligi = new ClaimsIdentity(kullaniciYetkileri, "Login");
                    ClaimsPrincipal principal = new(kullaniciKimligi);
                    await HttpContext.SignInAsync(principal);//HttpContext .net içerisinden geliyor ve uygulamanın çalışma anındaki içeriğe ulaşmamızı sağlıyor. SignInAsync metodu da .net içerisinde mevcut login işlemi yapmak istersek buradaki gibi kullanıyoruz
                    return Redirect("/Admin/Main");
                }
                else //eğer kullanıcı bilgileri yanlış girilmişse veya kullanıcı dbden silinmişse
                {
                    ModelState.AddModelError("", "Giriş Başarısız!");
                }
            }
            catch (Exception hata)
            {

                ModelState.AddModelError("", "Hata Oluştu!" + hata.Message);
            }
            return View();
        }
    }
}
