using aqay_apis.Context;
using Microsoft.EntityFrameworkCore;

namespace aqay_apis.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;
        private const int PageSize = 10;

        // constructor 
        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }
        // Read all categories as 10 categories by page
        public async Task<IEnumerable<Category>> getCategories(int page)
        {
            return await _context.Categories
                                 .Skip((page - 1) * PageSize)
                                 .Take(PageSize)
                                 .ToListAsync();
        }
        // Read Category by it's ID
        public async Task<Category> getCategoryById(int id)
        {
            return await _context.Categories.FindAsync(id);
        }
        // Read Category by it's name
        public async Task<Category> getCategoryByName(string name)
        {
            name = name.ToLower();
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Name == name);
            if ( category == null)
            {
                throw new Exception("Category not found.");
            }
            return category;
        }
        // Create a new category
        public async Task<Category> createCategory(string name)
        {
            name = name.ToLower();
            if (await _context.Categories.FirstOrDefaultAsync(c => c.Name == name) != null)
            {
                throw new Exception("Category already exists ");
            }
            var category = new Category
            {
                Name = name
            };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }
        // Update an existing category 
        public async Task<Category> updateCategory(string oldName, string newName)
        {
            oldName = oldName.ToLower();
            newName = newName.ToLower();
            var existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Name == oldName);
            if (existingCategory == null)
            {
                throw new Exception("Category not found.");
            }
            existingCategory.Name = newName;
            _context.Categories.Update(existingCategory);
            await _context.SaveChangesAsync();
            return existingCategory;
        }
        // Delete an existing category 
        public async Task<bool> deleteCategory(string name)
        {
            name.ToLower();
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Name == name);
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
