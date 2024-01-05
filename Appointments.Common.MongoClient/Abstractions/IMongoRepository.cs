using System.Linq.Expressions;

namespace Appointments.Common.MongoClient.Abstractions;

public interface IMongoRepository<TDocument>
    where TDocument : IMongoDocument
{
    Task<TDocument> CreateAsync(TDocument document);
    Task<IPagedResult<TDocument>> GetAsync(
        int pageIndex,
        int pageSize,
        string? sort = null,
        string? filter = null);
    Task<TDocument> GetOneAsync(Guid id);
    Task<TDocument> GetOneAsync(Expression<Func<TDocument, bool>> filter);
    Task<TDocument?> GetOneOrDefaultAsync(Guid id);
    Task<TDocument?> GetOneOrDefaultAsync(Expression<Func<TDocument, bool>> filter);
    Task UpdateAsync(TDocument document);
    Task DeleteAsync(Guid id);
}
