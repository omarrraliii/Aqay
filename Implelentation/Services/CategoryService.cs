using aqay_apis.Context;
using Microsoft.EntityFrameworkCore;

namespace aqay_apis.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;
        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Category>> getCategories()
        {
            var categories= await _context.Categories
                                 .ToListAsync();
            return categories;
        }
        public async Task<Category> getCategoryById(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) {
                throw new Exception("Category not found.");
            }
            return category;
        }
        public async Task<Category> getCategoryByName(string name)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());
            if ( category == null)
            {
                throw new Exception("Category not found.");
            }
            return category;
        }
        public async Task<Category> createCategory(string name)
        {
            if (await _context.Categories.FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower()) != null)
            {
                throw new Exception("Category already exists ");
            }
            var category = new Category
            {
                Name = name.ToLower()
            };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }
        public async Task<Category> updateCategory(string oldName, string newName)
        {
            var existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Name.ToLower() == oldName.ToLower());
            if (existingCategory == null)
            {
                throw new Exception("Category not found.");
            }
            existingCategory.Name = newName.ToLower();
            _context.Categories.Update(existingCategory);
            await _context.SaveChangesAsync();
            return existingCategory;
        }
        public async Task<bool> deleteCategory(string name)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());
            if (category == null)
            {
                return false;
                throw new Exception("Category not found.");
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
