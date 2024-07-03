namespace aqay_apis;

public class PaginatedResult<T>
{
    public IEnumerable<T> Items { get; set; }
    public int TotalCount { get; set; }
    public bool HasMoreItems { get; set; }
}
