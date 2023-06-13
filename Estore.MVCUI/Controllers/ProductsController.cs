using Estore.Core.Entities;
using Estore.MVCUI.Models;
using Estore.Service.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Estore.MVCUI.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _serviceProduct;

        public ProductsController(IProductService serviceProduct)
        {
            _serviceProduct = serviceProduct;
        }
        //[Route("tum-urunlerimiz")] //adres çubuğundan tum-urunlerimiz yazınca bu action çalışsın
        public async Task<IActionResult> Index()
        {
            var model = await _serviceProduct.GetAllAsync(p => p.IsActive);
            return View(model);
        }
        public async Task<IActionResult> Search(string q) //adres çubuğunda query string ile 
        {
            var model = await _serviceProduct.GetProductsByIncludeAsync(p => p.IsActive && p.Name.Contains(q) || p.Description.Contains(q) || p.Brand.Name.Contains(q) || p.Category.Name.Contains(q));
            return View(model);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var model = new ProductDetailViewModel();
            var product = await _serviceProduct.GetProductByIncludeAsync(id);
            model.Product = product;
            model.RelatedProducts = await _serviceProduct.GetAllAsync(p => p.CategoryId == product.CategoryId && p.Id != id);
            if (model is null)
            {
                return NotFound();
            }

            return View(model);
        }
    }
}
