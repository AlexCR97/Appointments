namespace Appointments.Domain.Models;

public sealed record PagedResult<T>(
    int PageIndex,
    int PageSize,
    long TotalCount,
    IReadOnlyList<T> Results);
