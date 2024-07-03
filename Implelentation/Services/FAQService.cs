using aqay_apis.Context;
using Microsoft.EntityFrameworkCore;

namespace aqay_apis;

public class FAQService:IFAQService
{
    private readonly ApplicationDbContext _context;
    private readonly GlobalVariables _globalVariables;
    public FAQService(ApplicationDbContext context,GlobalVariables globalVariables)
    {
        _context = context;
        _globalVariables = globalVariables;
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

    public async Task<PaginatedResult<FAQ>> GetAllFAQs(int pageIndex)
    {
        var FAQ=await _context.FAQs.Skip((pageIndex-1)*_globalVariables.PageSize)
                                    .Take(_globalVariables.PageSize)
                                    .ToListAsync();
        var FAQCount=FAQ.Count;
        var PaginatedResult=new PaginatedResult<FAQ>
        {
            Items=FAQ,
            TotalCount=FAQCount,
            HasMoreItems=(pageIndex*_globalVariables.PageSize)<FAQCount
        };
        return PaginatedResult;
    }

    public async Task<FAQ> GetFAQById(int id)
    {
        return await _context.FAQs.FindAsync(id);
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
