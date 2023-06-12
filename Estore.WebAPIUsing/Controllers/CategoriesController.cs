using Estore.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Estore.WebAPIUsing.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly HttpClient _httpClient;

        public CategoriesController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private readonly string _apiAdres = "https://localhost:7064/api/";
        public async Task<IActionResult> Index(int id)
        {
            var model = await _httpClient.GetFromJsonAsync<Category>(_apiAdres + "Categories/" + id);
            if (model == null)
            {
                return NotFound();
            }
           // model.Products = await _httpClient.GetFromJsonAsync<List<Product>>(_apiAdres + "Products");
           
            return View(model);
        }
    }
}
