using aqay_apis.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using aqay_apis.Context;

namespace aqay_apis
{
    public class TagService : ITagService
    {
        private readonly ApplicationDbContext _context;
        private readonly GlobalVariables _globalVariables;

        public TagService(ApplicationDbContext context, GlobalVariables globalVariables)
        {
            _context = context;
            _globalVariables = globalVariables;
        }


        public async Task<Tag> CreateTagAsync(string name)
        {
            var tag = new Tag
            {
                Name = name.ToLower()
            };

            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();
            return tag;
        }

        public async Task<Tag> GetTagByNameAsync(string name)
        {
            var tag= await _context.Tags.FirstOrDefaultAsync(t=>t.Name == name.ToLower());
            if (tag == null)
            {
                return null;
            }
            return tag;
        }

        public async Task<Tag> GetTagByIdAsync(int id)
        {
            var tag=await _context.Tags.FindAsync(id);
            if (tag == null)
            {
                return null;
                throw new Exception($"tag with id {id} is not found");
            }
            return tag;
        }

        public async Task<IEnumerable<Tag>> GetAllTagsAsync(int pageIndex)
        {
            var tags = await _context.Tags.OrderByDescending(t => t.Name)
                                           .Skip((pageIndex - 1) * _globalVariables.PageSize)
                                           .Take(_globalVariables.PageSize)
                                           .ToListAsync();
            var tagCount = await _context.Tags.CountAsync();
            var paginatedResult = new PaginatedResult<Tag>
            {
                Items = tags,
                TotalCount = tagCount,
                HasMoreItems = (pageIndex * _globalVariables.PageSize) < tagCount
            };
            return paginatedResult.Items;
        }

        public async Task<Tag> UpdateTagAsync(int id, string name)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null)
            {
                return null;
            }

            tag.Name = name.ToLower();

            _context.Tags.Update(tag);
            await _context.SaveChangesAsync();
            return tag;
        }

        public async Task<bool> DeleteTagAsync(int id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null)
            {
                return false;
            }

            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
