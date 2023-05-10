using Estore.Data;
using Estore.Data.Concrete;
using Estore.Service.Abstract;

namespace Estore.Service.Concrete
{
    public class ProductService : ProductRepository, IProductService
    {
        public ProductService(DatabaseContext context) : base(context)
        {
        }
    }
}
