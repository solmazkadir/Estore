using Estore.Core.Entities;
using Estore.MVCUI.ExtensionMethods;
using Estore.Service.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Estore.MVCUI.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService _serviceProduct;

        public CartController(IProductService serviceProduct)
        {
            _serviceProduct = serviceProduct;
        }

        public IActionResult Index()
        {
            return View(GetCart());
        }
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            var product = await _serviceProduct.FindAsync(productId);
            if (product != null)
            {
                var cart = GetCart();
                cart.AddProduct(product, quantity);
                SaveCart(cart);
            }
            return RedirectToAction("Index");
        }
        public IActionResult RemoveFromCart(int productId)
        {
            var product = _serviceProduct.Find(productId);
            if (product != null)
            {
                var cart = GetCart();
                cart.RemoveProduct(product);
                SaveCart(cart);
            }
            return RedirectToAction("Index");
        }
        public void SaveCart(Cart cart)
        {
            HttpContext.Session.SetJson("Cart", cart);
        }
        public Cart GetCart() // geriye cart nesnemizi döndüren metot
        {
            return HttpContext.Session.GetJson<Cart>("Cart") ?? new Cart(); //session a bakıyoruz  eğer cart isminde session varsa onu yoksa yeni bir cart nesnesi döndürüyoruz
        }
    }
}
