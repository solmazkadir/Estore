using Estore.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Estore.WebAPIUsing.Controllers
{
    public class ProductsController : Controller
    {
        private readonly HttpClient _httpClient;

        public ProductsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private readonly string _apiAdres = "https://localhost:7064/api/Products";
        [Route("tum-urunlerimiz")]
        public async Task<IActionResult> Index()
        {
            return View(await _httpClient.GetFromJsonAsync<List<Product>>(_apiAdres));
        }
    }
}
