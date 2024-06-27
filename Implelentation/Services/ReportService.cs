using aqay_apis.Context;
using Microsoft.EntityFrameworkCore;

namespace aqay_apis;

public class ReportService:IReportService
{
    private readonly ApplicationDbContext _context;
    private readonly GlobalVariables _globalVariables;
    public ReportService(ApplicationDbContext context, GlobalVariables globalVariables)
    {
        _context = context;
        _globalVariables = globalVariables;
    }
    public async Task<Report> CreateReportAsync(string title,string intiatorId,string description)
    {
        var report = new Report
        {
            Title = title,
            IntiatorId=intiatorId,
            Description = description,
            CreatedOn=DateTime.Now,
            LastEdit=DateTime.Now,
            REPORTSTATUS=REPORTSTATUSES.OPEN
        };
        await _context.Reports.AddAsync(report);
        await _context.SaveChangesAsync();
        return report;
    }
    public async Task<ICollection<Report>> GetReportsAsync(int pageIndex)
    {
        return await _context.Reports
                              .OrderByDescending(r=>r.CreatedOn)
                              .Skip((pageIndex - 1) * _globalVariables.PageSize)
                              .Take(_globalVariables.PageSize)
                              .ToListAsync();
    }
    public async Task<Report> GetReportByIdAsync (int id)
    {
        var report=await _context.Reports.FindAsync(id);
        if (report == null)
        {
            throw new Exception($"Failed to find report with id {id}");
        }
        return report;
    }
    public async Task<Report> GetReportByTitleAsync (string title)
    {
        title=title.ToLower();
        var report = await _context.Reports.FirstOrDefaultAsync(t=>t.Title == title);
        if (report == null)
        {
            throw new Exception ($"Failed to find report with title {title}");
        }
        return report;
    }
    public async Task<ICollection<Report>> GetReportsByStatusAsync(REPORTSTATUSES REPORTSTATUS,int pageIndex)
    {
        List<Report> reports = new List<Report>();
        if (REPORTSTATUS == REPORTSTATUSES.OPEN)
        {
            reports=await _context.Reports
                                .Skip((pageIndex-1)*_globalVariables.PageSize)
                                .Take(_globalVariables.PageSize)
                                .Where(r=>r.REPORTSTATUS==REPORTSTATUSES.OPEN)
                                .ToListAsync();
        }
        else if (REPORTSTATUS == REPORTSTATUSES.CLOSED)
        {
            reports= await _context.Reports
                                .Skip((pageIndex-1)*_globalVariables.PageSize)
                                .Take(_globalVariables.PageSize)
                                .Where(r=>r.REPORTSTATUS==REPORTSTATUSES.CLOSED)
                                .ToListAsync();
        }
        return reports;
    }

    public async Task<Report> OpenReportAsync(int id, string reviewerId)
    {
        var report = await _context.Reports.FindAsync(id);
        if (report.ReviewerId == null)
        {
            report.ReviewerId = reviewerId;
            await _context.SaveChangesAsync();
            return report;
        }
        
        return report;
    }

    public async Task<Report> UpdateReportAsync(int id,string action, REPORTSTATUSES REPORTSTATUS)
    {
        var report= await _context.Reports.FindAsync(id);
        report.Action = action;
        report.REPORTSTATUS = REPORTSTATUS;
        report.LastEdit= DateTime.Now;
        await _context.SaveChangesAsync();
        return report;
    }

    public async Task<bool> DeleteReportAsync(int id)
    {
        var report= await _context.Reports.FindAsync(id);
        if (report==null)
        {
            return false;
            throw new Exception("No report found");
        }
        _context.Remove(report);
        await _context.SaveChangesAsync();
        return true;

    }
}
