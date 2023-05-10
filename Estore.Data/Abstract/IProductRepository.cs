using Estore.Core.Entities;
using System.Linq.Expressions;

namespace Estore.Data.Abstract
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product> GetProductByIncludeAsync(int id); // bu metot bize ürüne marka ve kategori include edilmiş veritabanından 1 tane kayıt getirecek
        Task<List<Product>> GetProductsByIncludeAsync(); // tüm ürünleri marka ve kategorisiyle getirecek metot
        Task<List<Product>> GetProductsByIncludeAsync(Expression<Func<Product, bool>> expression); //tüm ürünleri marka ve kategorisiyle filtre uygulayarak getirecek metot
    }
}
