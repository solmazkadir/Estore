using Estore.Core.Entities;
using Estore.MVCUI.Models;
using Estore.Service.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Estore.MVCUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IService<AppUser> _serviceAppUser;

        public AccountController(IService<AppUser> serviceAppUser)
        {
            _serviceAppUser = serviceAppUser;
        }

        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("userId");
            if (userId == null)
            {
                TempData["Message"] = "<div class='alert alert-danger'>Lütfen Giriş Yapınız</div>";
                return RedirectToAction("Login");
            }
            else
            {
                var user = await _serviceAppUser.GetAsync(u => u.Id == userId);
                return View(user);
            }

        }
        [HttpPost]
        public async Task<IActionResult> UpdateUser(AppUser appUser)
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("userId");
                var user = await _serviceAppUser.GetAsync(u => u.Id == userId);
                if (user is not null)
                {
                    user.Surname = appUser.Surname;
                    user.Name = appUser.Name;
                    user.Email = appUser.Email;
                    user.Phone = appUser.Phone;
                    user.Password = appUser.Password;
                    _serviceAppUser.Update(user);
                    await _serviceAppUser.SaveAsync();
                    return RedirectToAction(nameof(Index));
                }

            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Hata Oluştu!");

            }
            return View("Index", appUser);
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginViewModel viewModel)
        {
            var user = await _serviceAppUser.GetAsync(x => x.Email == viewModel.Email && x.Password == viewModel.Password && x.IsActive);
            if (user == null)
            {
                ModelState.AddModelError("", "Giriş Başarısız");
            }
            else
            {
                HttpContext.Session.SetInt32("userId", user.Id);
                HttpContext.Session.SetString("userGuid", user.UserGuid.ToString());
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(AppUser appUser)
        {

            try
            {
                var kullanici = await _serviceAppUser.GetAsync(x => x.Email == appUser.Email);
                if (kullanici != null)
                {
                    ModelState.AddModelError("", "Bu Mail Adresi Zaten Kayıtlı");
                    return View();
                }
                else
                {
                    appUser.UserGuid = Guid.NewGuid();
                    appUser.IsActive = true;
                    appUser.IsAdmin = false;
                    await _serviceAppUser.AddAsync(appUser);
                    await _serviceAppUser.SaveAsync();
                    TempData["Message"] = "<div class='alert alert-success'>Kayıt Başarılı. Giriş Yapabilirsiniz...</div>";
                    return RedirectToAction("Login");
                }

            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Hata Oluştu!");
            }

            return RedirectToAction("Login");

        }
        public IActionResult Logout()
        {
            try
            {
                HttpContext.Session.Remove("userId");
                HttpContext.Session.Remove("userGuid");
            }
            catch
            {
                HttpContext.Session.Clear();
            }

            return RedirectToAction("Index", "Home");
        }
        public IActionResult NewPassword()
        {
            return View();
        }
    }
}
