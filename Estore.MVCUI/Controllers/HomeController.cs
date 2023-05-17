using Estore.Core.Entities;
using Estore.MVCUI.Models;
using Estore.Service.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Estore.MVCUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IService<Slider> _serviceSlider;
        private readonly IService<Product> _serviceProduct;

        public HomeController(IService<Slider> serviceSlider, IService<Product> serviceProduct)
        {
            _serviceSlider = serviceSlider;
            _serviceProduct = serviceProduct;
        }

        public async Task<IActionResult> Index()
        {
            var model = new HomePageViewModel()
            {
                Sliders = await _serviceSlider.GetAllAsync(),
                Products = await _serviceProduct.GetAllAsync(p => p.IsActive && p.IsHome)
            };
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [Route("AccessDenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}