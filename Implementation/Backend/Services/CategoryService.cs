using Aqay_v2.Context;
using Aqay_v2.Models;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace Aqay_v2.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public CategoryService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<Category> Create(Category category)
        {
            await _applicationDbContext.AddAsync(category);
            _applicationDbContext.SaveChanges();
            return category;
        }

        public Category Delete(Category category)
        {
            _applicationDbContext.Remove(category);
            _applicationDbContext.SaveChanges();
            return category;
        }

        public async Task<IEnumerable<Category>> ReadAll()
        {
            return await _applicationDbContext.Categories.OrderBy(category => category.Name).ToListAsync();
        }

        public async Task<Category> ReadById(int Id)
        {
            var category = await _applicationDbContext.Categories.SingleOrDefaultAsync(c => c.Id == Id);

            if (category == null)
            {
                // Throw an InvalidOperationException indicating that the category with the specified name was not found.
                throw new InvalidOperationException($"Category with id '{Id}' not found");
            }
            return category;
        }

        public async Task<Category> ReadByName(string name)
        {
            var category = await _applicationDbContext.Categories.SingleOrDefaultAsync(c => c.Name == name);

            if (category == null)
            {
                // Throw an InvalidOperationException indicating that the category with the specified name was not found.
                throw new InvalidOperationException($"Category with name '{name}' not found");
            }
            return category;
        }

        public Category Update(Category category)
        {
            _applicationDbContext.Update(category);
            _applicationDbContext.SaveChanges();
            return category;
        }
    }
}