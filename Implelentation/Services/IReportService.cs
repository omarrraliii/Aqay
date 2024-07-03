namespace aqay_apis;

public interface IReportService
{
    Task<Report> CreateReportAsync(string title,string intiatorId,string description);
    Task<ICollection<Report>> GetReportsAsync(int page);
    Task<Report> GetReportByIdAsync(int id);
    Task<IEnumerable<Report>> GetReportByTitleAsync(string title);
    Task<ICollection<Report>> GetReportsByStatusAsync(REPORTSTATUSES REPORTSTATUS,int pageIndex);
    Task<Report> OpenReportAsync(int id,string reviewerId);
    Task<Report> UpdateReportAsync(int id,string action,REPORTSTATUSES REPORTSTATUS);
    Task<bool> DeleteReportAsync(int id);
}
