namespace aqay_apis;

public class Report
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? ReviewerId { get; set; }
    public string IntiatorId { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime LastEdit { get; set; }
    public string Description { get; set; }
    public string? Action { get; set; }
    public REPORTSTATUSES REPORTSTATUS { get; set; }

}
