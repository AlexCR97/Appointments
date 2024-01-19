using Appointments.Assets.Application;
using Appointments.Assets.Domain;
using Appointments.Common.Domain.Exceptions;
using Appointments.Common.Domain.Models;
using Appointments.Common.MongoClient.Abstractions;

namespace Appointments.Assets.Infrastructure.Mongo.Assets;

internal sealed class AssetRepository : IAssetRepository
{
    private readonly IMongoRepository<AssetDocument> _repository;

    public AssetRepository(IMongoRepository<AssetDocument> repository)
    {
        _repository = repository;
    }

    public async Task<Asset> CreateAsync(Asset entity)
    {
        var document = AssetDocument.From(entity);
        var createdDocument = await _repository.CreateAsync(document);
        return createdDocument.ToEntity();
    }

    public async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<PagedResult<Asset>> FindAsync(int pageIndex, int pageSize, string? sort, string? filter)
    {
        var documents = await _repository.GetAsync(
            pageIndex,
            pageSize,
            sort: sort,
            filter: filter);

        return new PagedResult<Asset>(
            pageIndex,
            pageSize,
            documents.TotalCount,
            documents.Results
                .Select(x => x.ToEntity())
                .ToList());
    }

    public async Task<Asset> GetAsync(Guid id)
    {
        try
        {
            var document = await _repository.GetOneAsync(id);
            return document.ToEntity();
        }
        catch (Exception ex)
        {
            if (ex is Common.MongoClient.Exceptions.NotFoundException<AssetDocument>)
                throw new NotFoundException(nameof(Asset), "ID", id.ToString());

            throw;
        }
    }

    public async Task<Asset> GetByPathAsync(AssetPath path)
    {
        return await GetByPathOrDefaultAsync(path)
            ?? throw new NotFoundException(nameof(Asset), nameof(Asset.Path), path.Value);
    }

    public async Task<Asset?> GetByPathOrDefaultAsync(AssetPath path)
    {
        var document = await _repository.GetOneOrDefaultAsync(x => x.Path == path.Value);
        return document?.ToEntity();
    }

    public async Task<Asset> GetByTransactionCode(string code)
    {
        var document = await _repository.GetOneOrDefaultAsync(x => x.Transaction.Code == code)
            ?? throw new NotFoundException(nameof(Asset), "Transaction.Code", code);

        return document.ToEntity();
    }

    public async Task<Asset?> GetOrDefaultAsync(Guid id)
    {
        var document = await _repository.GetOneOrDefaultAsync(id);
        return document?.ToEntity();
    }

    public async Task UpdateAsync(Asset entity)
    {
        var document = AssetDocument.From(entity);
        await _repository.UpdateAsync(document);
    }
}
