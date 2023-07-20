namespace Appointments.Common.MongoClient.Abstractions;

public interface IPagedResult<TDocument>
{
    int PageIndex { get; }
    int PageSize { get; }
    long TotalCount { get; }
    IReadOnlyList<TDocument> Results { get; }
}
