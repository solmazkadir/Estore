using Estore.Core.Entities;

namespace Estore.Data.Abstract
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Category> GetCategoryByIncludeAsync(int id);
    }
}
