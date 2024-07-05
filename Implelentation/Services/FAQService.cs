using aqay_apis.Context;
using Microsoft.EntityFrameworkCore;
namespace aqay_apis;
public class FAQService:IFAQService
{
    private readonly ApplicationDbContext _context;
    public FAQService(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<FAQ> CreateFAQ(string question,string answer)
    {
        var FAQ=new FAQ
        {
            Question = question,
            Answer = answer
        };
        _context.FAQs.Add(FAQ);
        await _context.SaveChangesAsync();
        return FAQ;
    }
    public async Task<bool> DeleteFAQ(int id)
    {
        var FAQ = await _context.FAQs.FindAsync(id);
        if (FAQ == null)
        {
            return false;
            throw new Exception($"There's no Question with id {id}");
        }
        _context.FAQs.Remove(FAQ);
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<IEnumerable<FAQ>> GetAllFAQs()
    {
        return await _context.FAQs.ToListAsync();

    }
    public async Task<FAQ> GetFAQById(int id)
    {
        var faq = await _context.FAQs.FindAsync(id);
        if (faq == null)
        {
            throw new Exception("FAQ not found");
        }
        return faq;
    }
    public async Task<IEnumerable<FAQ>> SearchFAQs(string query)
    {
        var FAQs=await _context.FAQs.Where(q=>EF.Functions.Like(q.Question,$"%{query}%")
                                            ||EF.Functions.Like(q.Answer,$"%{query}%"))
                                            .ToListAsync();
        return FAQs;
    }
    public async Task<FAQ> UpdateFAQ(int id, string question,string answer)
    {
        var FAQ = await _context.FAQs.FindAsync(id);
        if (FAQ==null)
        {
            return null;
        }
        FAQ.Question=question;
        FAQ.Answer=answer;
        await _context.SaveChangesAsync();
        return FAQ;
    }

}
