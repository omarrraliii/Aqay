using Aqay_v2.Models;

namespace Aqay_v2.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> ReadAll();
        Task<Category> ReadById(int Id);
        Task<Category> ReadByName(string name);
        Task<Category> Create(Category category);
        Category Update(Category category);
        Category Delete(Category category);
    }
}
