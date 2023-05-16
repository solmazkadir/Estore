using Estore.Core.Entities;
using Estore.MVCUI.Utils;
using Estore.Service.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Estore.MVCUI.Areas.Admin.Controllers
{
    [Area("Admin"),Authorize(Policy = "AdminPolicy")]
    public class BrandsController : Controller
    {
        private readonly IService<Brand> _service; //readonly nesneler sadece constructor metotta doldurulabilir

        public BrandsController(IService<Brand> service)
        {
            _service = service;
        }

        // GET: BrandsController
        public ActionResult Index()
        {
            var model = _service.GetAll();

            return View(model);
        }

        // GET: BrandsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: BrandsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BrandsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(Brand collection, IFormFile? Logo)
        {
            try
            {
                if (Logo is not null)
                {
                    collection.Logo = await FileHelper.FileLoaderAsync(Logo);
                }
                await _service.AddAsync(collection);
                await _service.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BrandsController/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null) // id gönderilmeden direkt edit sayfası açılırsa
            {
                return BadRequest(); //geriye geçersiz istek hatası döndür
            }
            var model = await _service.FindAsync(id.Value); // yukarıdaki id yi ? ile nullable yaparsak
            if (model == null)
            {
                return NotFound(); // kayıt bulunamadı : 404
            }
            return View(model);
        }

        // POST: BrandsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Brand collection, IFormFile? Logo, bool? resmiSil)
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
                _service.Update(collection);
                _service.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BrandsController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {

            var model = await _service.FindAsync(id);

            return View(model);
        }

        // POST: BrandsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, Brand collection)
        {
            try
            {
                FileHelper.FileRemover(collection.Logo);
                _service.Delete(collection);
                await _service.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
