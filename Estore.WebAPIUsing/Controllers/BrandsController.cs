using Estore.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Estore.WebAPIUsing.Controllers
{
    public class BrandsController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiAdres = "https://localhost:7064/api/Brands/";
        public BrandsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }
            var model = await _httpClient.GetFromJsonAsync<Brand>(_apiAdres+id.Value);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }
    }
}
