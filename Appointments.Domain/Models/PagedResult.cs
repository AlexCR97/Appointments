namespace Appointments.Domain.Models;

public class PagedResult<T>
{
    public int PageIndex { get; }
    public int PageSize { get; }
    public long TotalCount { get; }
    public IReadOnlyList<T> Results { get; }

    public PagedResult(int pageIndex, int pageSize, long totalCount, IReadOnlyList<T> results)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        TotalCount = totalCount;
        Results = results;
    }
}
