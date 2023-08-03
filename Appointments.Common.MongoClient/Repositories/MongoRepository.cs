using Appointments.Common.MongoClient.Abstractions;
using Appointments.Common.MongoClient.Exceptions;
using Appointments.Common.MongoClient.Extensions.Filters;
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

        var results = await _collection
            .Find(BuildFilterExpression(filter))
            .Sort(BuildSortDefinition(sort))
            .Skip(pageIndex * pageSize)
            .Limit(pageSize)
            .ToListAsync();

        return new PagedResult<TDocument>(
            pageIndex,
            pageSize,
            totalCount,
            results);
    }

    private static Expression<Func<TDocument, bool>> BuildFilterExpression(string? filter)
    {
        try
        {
            return string.IsNullOrWhiteSpace(filter)
                ? _ => true
                : filter.ToExpression<TDocument>();
        }
        catch (Exception ex)
        {
            throw new Exceptions.MongoException(
                "InvalidFilter",
                @"A valid filter must have the format of an expression. E.g.: ""age == 10 or name.toLower().contains('Some value')""",
                ex);
        }
    }

    private static SortDefinition<TDocument> BuildSortDefinition(string? sort)
    {
        var sortDefinitionBuilder = new SortDefinitionBuilder<TDocument>();

        if (string.IsNullOrWhiteSpace(sort))
            return sortDefinitionBuilder.Combine();

        var sortParts = sort
            .Split(' ')
            .ToList();

        if (sortParts.Count == 1)
        {
            var sortProperty = sortParts[0];
            return sortDefinitionBuilder.Ascending(sortProperty);
        }

        if (sortParts.Count == 2)
        {
            var sortProperty = sortParts[0];
            var sortDirection = sortParts[1].ToLower();

            return string.IsNullOrWhiteSpace(sortDirection) || sortDirection == "asc"
                ? sortDefinitionBuilder.Ascending(sortProperty)
                : sortDefinitionBuilder.Descending(sortProperty);
        }

        throw new Exceptions.MongoException(
            "InvalidSort",
            @"A valid sort must have the format ""property direction"", where direction is asc/desc. E.g.: ""createdAt desc""");
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
