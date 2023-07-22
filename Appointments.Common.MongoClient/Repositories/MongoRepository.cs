using Appointments.Common.MongoClient.Abstractions;
using Appointments.Common.MongoClient.Exceptions;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Appointments.Common.MongoClient.Repositories;

internal class MongoRepository<TDocument> : IMongoRepository<TDocument>
    where TDocument : IMongoDocument
{
    private readonly IMongoCollection<TDocument> _collection;

    public MongoRepository(IMongoCollection<TDocument> collection)
    {
        _collection = collection;
    }

    public async Task<TDocument> CreateAsync(TDocument document)
    {
        if (document.Id == Guid.Empty)
            document.Id = Guid.NewGuid();

        if (document.CreatedAt == default)
            document.CreatedAt = DateTime.UtcNow;

        await _collection.InsertOneAsync(document);

        return document;
    }

    public async Task<IPagedResult<TDocument>> GetAsync(
        int pageIndex,
        int pageSize,
        string? sort = null,
        string? filter = null)
    {
        var totalCount = await _collection
            .CountDocumentsAsync(_ => true);

        FilterDefinition<TDocument> mongoFilter = string.IsNullOrWhiteSpace(filter)
            ? "{}"
            : filter;

        var results = await _collection
            .Find(mongoFilter)
            .Skip(pageIndex * pageSize)
            .Limit(pageSize)
            .ToListAsync();

        return new PagedResult<TDocument>(
            pageIndex,
            pageSize,
            totalCount,
            results);
    }

    public async Task<TDocument> GetOneAsync(Guid id)
    {
        return await _collection
            .Find(x => x.Id == id)
            .FirstOrDefaultAsync()
            ?? throw new NotFoundException<TDocument>(id);
    }

    public async Task<TDocument> GetOneAsync(Expression<Func<TDocument, bool>> filter)
    {
        return await _collection
            .Find(filter)
            .FirstOrDefaultAsync()
            ?? throw new NotFoundException<TDocument>();
    }

    public async Task<TDocument?> GetOneOrDefaultAsync(Guid id)
    {
        return await _collection
            .Find(x => x.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<TDocument?> GetOneOrDefaultAsync(Expression<Func<TDocument, bool>> filter)
    {
        return await _collection
            .Find(filter)
            .FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(TDocument document)
    {
        if (document.UpdatedAt == default)
            document.UpdatedAt = DateTime.UtcNow;

        await _collection.FindOneAndReplaceAsync(x => x.Id == document.Id, document);
    }

    public async Task DeleteAsync(Guid id)
    {
        var document = await GetOneAsync(id);
        
        if (document.DeletedAt == default)
            document.DeletedAt = DateTime.UtcNow;

        await _collection.FindOneAndReplaceAsync(x => x.Id == document.Id, document);
    }

    public async Task PurgeAsync(Guid id)
    {
        await _collection.FindOneAndDeleteAsync(x => x.Id == id);
    }
}
