using Estore.Core.Entities;
using Estore.WebAPIUsing.Models;
using Microsoft.AspNetCore.Mvc;

namespace Estore.WebAPIUsing.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;

        public AccountController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private readonly string _apiAdres = "https://localhost:7064/api/AppUsers";
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
                var user = await _httpClient.GetFromJsonAsync<AppUser>(_apiAdres + "/" + userId);
                return View(user);
            }

        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LoginAsync(UserLoginViewModel viewModel)
        {
            var users = await _httpClient.GetFromJsonAsync<List<AppUser>>(_apiAdres);
            var user = users.FirstOrDefault(x => x.Email == viewModel.Email && x.Password == viewModel.Password && x.IsActive);
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
                var users = await _httpClient.GetFromJsonAsync<List<AppUser>>(_apiAdres);
                var kullanici = users.FirstOrDefault(x => x.Email == appUser.Email);
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
                    var sonuc = await _httpClient.PostAsJsonAsync(_apiAdres, appUser);
                    if (sonuc.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "<div class='alert alert-success'>Kayıt Başarılı. Giriş Yapabilirsiniz...</div>";
                        return RedirectToAction("Login");
                    }

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
        [HttpPost]
        public async Task<IActionResult> UpdateUser(AppUser appUser)
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("userId");
                var user = await _httpClient.GetFromJsonAsync<AppUser>(_apiAdres + "/" + userId);
                if (user is not null)
                {
                   
                    user.Surname = appUser.Surname;
                    user.Name = appUser.Name;
                    user.Email = appUser.Email;
                    user.Phone = appUser.Phone;
                    user.Password = appUser.Password;
                    var response = await _httpClient.PutAsJsonAsync(_apiAdres, user);
                    if (response.IsSuccessStatusCode) //api den başarılı bir istek kodu geldiyse(200 ok)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    
                }

            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Hata Oluştu!");

            }
            return View("Index", appUser);
        }

    }
}
