using aqay_apis.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aqay_apis
{
    public interface ITagService
    {
        Task<Tag> CreateTagAsync(string name);
        Task<Tag> GetTagByNameAsync(string name);
        Task<Tag> GetTagByIdAsync(int id);
        Task<IEnumerable<Tag>> GetAllTagsAsync(int pageIndex);
        Task<Tag> UpdateTagAsync(int id, string name);
        Task<bool> DeleteTagAsync(int id);
    }
}
