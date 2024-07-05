using System.Collections.Generic;
using System.Threading.Tasks;

namespace aqay_apis
{
    public interface IFAQService
    {
        Task<IEnumerable<FAQ>> GetAllFAQs();
        Task<FAQ> GetFAQById(int id);
        Task<FAQ> CreateFAQ(string question,string answer);
        Task<FAQ> UpdateFAQ(int id,string question,string answer);
        Task<bool> DeleteFAQ(int id);
        Task<IEnumerable<FAQ>> SearchFAQs(string query);
    }
}
