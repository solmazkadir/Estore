using Estore.Core.Entities;
using Estore.WebAPIUsing.Models;
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

        public async Task<IActionResult> Search(string q) //adres çubuğunda query string ile 
        {
            var products = await _httpClient.GetFromJsonAsync<List<Product>>(_apiAdres + "/GetSearch/" + q);

            
            return View(products);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var model = new ProductDetailViewModel();
            var products = await _httpClient.GetFromJsonAsync<List<Product>>(_apiAdres);
            var product = await _httpClient.GetFromJsonAsync<Product>(_apiAdres + "/" + id); //await _serviceProduct.GetProductByIncludeAsync(id);
            model.Product = product;
            model.RelatedProducts = products.Where(p => p.CategoryId == product.CategoryId && p.Id != id).ToList();
            if (model is null)
            {
                return NotFound();
            }

            return View(model);
        }
    }
}
