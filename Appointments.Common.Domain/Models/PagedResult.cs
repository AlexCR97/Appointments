namespace Appointments.Common.Domain.Models;

public sealed record PagedResult<T>(
    int PageIndex,
    int PageSize,
    long TotalCount,
    IReadOnlyList<T> Results)
{
    public PagedResult<U> Map<U>(Func<T, U> mapFunction)
    {
        return new PagedResult<U>(
            PageIndex,
            PageSize,
            TotalCount,
            Results
                .Select(mapFunction)
                .ToList());
    }
}
