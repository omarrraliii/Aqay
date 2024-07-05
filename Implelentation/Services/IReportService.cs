namespace aqay_apis;
public interface IReportService
{
    Task<Report> CreateReportAsync(string title,string intiatorId,string description);
    Task<IEnumerable<Report>> GetReportsAsync();
    Task<Report> GetReportByIdAsync(int id);
    Task<IEnumerable<Report>> GetReportByTitleAsync(string title);
    Task<IEnumerable<Report>> GetReportsByStatusAsync(REPORTSTATUSES REPORTSTATUS);
    Task<Report> OpenReportAsync(int id,string reviewerId);
    Task<Report> UpdateReportActionAsync(int id,string action);
    Task<Report> UpdateReportStatusAsync(int id);
    Task<bool> DeleteReportAsync(int id);
}
