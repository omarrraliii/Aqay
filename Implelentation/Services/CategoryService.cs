using aqay_apis.Context;
using Microsoft.EntityFrameworkCore;

namespace aqay_apis.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly GlobalVariables _globalVariable;

        // constructor 
        public CategoryService(ApplicationDbContext context, GlobalVariables globalVariable)
        {
            _context = context;
            _globalVariable = globalVariable;
        }
        // Read all categories as 10 categories by page
        public async Task<PaginatedResult<Category>> getCategories(int page)
        {
            var categories= await _context.Categories
                                 .Skip((page - 1) * _globalVariable.PageSize)
                                 .Take(_globalVariable.PageSize)
                                 .ToListAsync();
            var categorieCount= await _context.Categories.CountAsync();
            var paginatedResult=new PaginatedResult<Category>
            {
                Items=categories,
                TotalCount=categorieCount,
                HasMoreItems=page*_globalVariable.PageSize<categorieCount
            };
            return paginatedResult;
        }
        // Read Category by it's ID
        public async Task<Category> getCategoryById(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) {
                throw new Exception("Category not found.");
            }
            return category;
        }
        // Read Category by it's name
        public async Task<Category> getCategoryByName(string name)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());
            if ( category == null)
            {
                throw new Exception("Category not found.");
            }
            return category;
        }
        // Create a new category
        public async Task<Category> createCategory(string name)
        {
            if (await _context.Categories.FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower()) != null)
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
            var existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Name.ToLower() == oldName.ToLower());
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
