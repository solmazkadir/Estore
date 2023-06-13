using Estore.Core.Entities;
using Estore.Service.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Estore.MVCUI.Controllers
{
    public class NewsController : Controller
    {
        private readonly IService<News> _serviceNews;

        public NewsController(IService<News> serviceNews)
        {
            _serviceNews = serviceNews;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _serviceNews.GetAllAsync(n => n.IsActive && n.IsHome));
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var model = await _serviceNews.FindAsync(id.Value);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }
    }
}
