namespace aqay_apis.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> getCategories(int page);
        Task<Category> getCategoryById(int id);
        Task<Category> getCategoryByName(string name);
        Task<Category> createCategory(string name);
        Task<Category> updateCategory(string oldName, string newName);
        Task<bool> deleteCategory(string name);
    }
}
