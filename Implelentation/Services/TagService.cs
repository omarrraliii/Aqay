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
        public TagService(ApplicationDbContext context)
        {
            _context = context;
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
            return await _context.Tags.OrderByDescending(t => t.Name)
                                           .ToListAsync();
           
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
