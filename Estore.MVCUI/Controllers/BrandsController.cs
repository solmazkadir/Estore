using Estore.Core.Entities;
using Estore.Service.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Estore.MVCUI.Controllers
{
    public class BrandsController : Controller
    {
        private readonly IService<Brand> _serviceBrand;
        private readonly IService<Product> _serviceProduct;

        public BrandsController(IService<Brand> serviceBrand, IService<Product> serviceProduct)
        {
            _serviceBrand = serviceBrand;
            _serviceProduct = serviceProduct;
        }

        public async Task<IActionResult> Index(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }
            var model = await _serviceBrand.FindAsync(id.Value);
            if (model == null)
            {
                return NotFound();
            }
            model.Products =await _serviceProduct.GetAllAsync(p => p.BrandId == id.Value);
            return View(model);
        }
    }
}
