using Estore.Core.Entities;

namespace Estore.MVCUI.Models
{
    public class ProductDetailViewModel
    {
        public Product Product { get; set; }
        public List<Product>? RelatedProducts { get; set; }
    }
}
