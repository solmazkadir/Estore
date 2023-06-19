using Estore.Core.Entities;
using Estore.WebAPIUsing.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Estore.WebAPIUsing.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Policy = "AdminPolicy")]
    public class SettingsController : Controller
    {
        private readonly HttpClient _httpClient;

        public SettingsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        private readonly string _apiAdres = "https://localhost:7064/api/Settings";
        // GET: SettingsController
        public async Task<ActionResult> Index()
        {
            var model = await _httpClient.GetFromJsonAsync<List<Setting>>(_apiAdres);
            return View(model);
        }

        // GET: SettingsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SettingsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SettingsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Setting collection, IFormFile? Logo, IFormFile? Favicon)
        {
            try
            {

                if (Logo is not null)
                {
                    collection.Logo = await FileHelper.FileLoaderAsync(Logo);
                } 
                if (Favicon is not null)
                {
                    collection.Favicon = await FileHelper.FileLoaderAsync(Favicon);
                }
                var response = await _httpClient.PostAsJsonAsync(_apiAdres, collection);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

            }
            catch
            {

                ModelState.AddModelError("", "Hata Oluştu!");

            }
            return View();
        }

        // GET: SettingsController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var model = await _httpClient.GetFromJsonAsync<Setting>(_apiAdres + "/" + id);
            return View(model);
        }

        // POST: SettingsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Setting collection, IFormFile? Logo, IFormFile? Favicon, bool? resmiSil)
        {
            try
            {
                if (resmiSil is not null && resmiSil == true)
                {
                    FileHelper.FileRemover(collection.Logo);
                    collection.Logo = "";
                }

                if (Logo is not null)
                {
                    collection.Logo = await FileHelper.FileLoaderAsync(Logo);
                }
                if (Favicon is not null)
                {
                    collection.Favicon = await FileHelper.FileLoaderAsync(Favicon);
                }
                var response = await _httpClient.PutAsJsonAsync(_apiAdres, collection);
                if (response.IsSuccessStatusCode) //api den başarılı bir istek kodu geldiyse(200 ok)
                {
                    return RedirectToAction(nameof(Index));
                }

            }
            catch
            {
                ModelState.AddModelError("", "Hata Oluştu!");
            }

            return View();
        }

        // GET: SettingsController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {

            var model = await _httpClient.GetFromJsonAsync<Setting>(_apiAdres + "/" + id);
            return View(model);
        }

        // POST: SettingsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, Setting collection)
        {
            try
            {
                FileHelper.FileRemover(collection.Logo);
                FileHelper.FileRemover(collection.Favicon);
                await _httpClient.DeleteAsync(_apiAdres + "/" + id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
