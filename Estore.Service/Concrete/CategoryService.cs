using Estore.Data;
using Estore.Data.Concrete;
using Estore.Service.Abstract;

namespace Estore.Service.Concrete
{
    public class CategoryService : CategoryRepository, ICategoryService
    {
        public CategoryService(DatabaseContext context) : base(context)
        {
        }
    }
}
