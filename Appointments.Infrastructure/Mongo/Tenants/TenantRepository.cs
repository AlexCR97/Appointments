using Appointments.Common.Domain.Exceptions;
using Appointments.Common.Domain.Models;
using Appointments.Common.MongoClient.Abstractions;
using Appointments.Core.Application.Requests.Tenants;
using Appointments.Core.Domain.Entities;

namespace Appointments.Core.Infrastructure.Mongo.Tenants;

internal sealed class TenantRepository : ITenantRepository
{
    private readonly IMongoRepository<TenantDocument> _repository;

    public TenantRepository(IMongoRepository<TenantDocument> repository)
    {
        _repository = repository;
    }

    public async Task<Tenant> CreateAsync(Tenant entity)
    {
        var document = TenantDocument.From(entity);
        var createdDocument = await _repository.CreateAsync(document);
        return createdDocument.ToEntity();
    }

    public async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<PagedResult<Tenant>> FindAsync(int pageIndex, int pageSize, string? sort, string? filter)
    {
        var documents = await _repository.GetAsync(
            pageIndex,
            pageSize,
            sort: sort,
            filter: filter);

        return new PagedResult<Tenant>(
            pageIndex,
            pageSize,
            documents.TotalCount,
            documents.Results
                .Select(x => x.ToEntity())
                .ToList());
    }

    public async Task<Tenant> GetAsync(Guid id)
    {
        try
        {
            var document = await _repository.GetOneAsync(id);
            return document.ToEntity();
        }
        catch (Exception ex)
        {
            if (ex is Common.MongoClient.Exceptions.NotFoundException<TenantDocument>)
                throw new NotFoundException(nameof(Tenant), "ID", id.ToString());

            throw;
        }
    }

    public async Task<Tenant?> GetOrDefaultAsync(Guid id)
    {
        var document = await _repository.GetOneOrDefaultAsync(id);
        return document?.ToEntity();
    }

    public async Task UpdateAsync(Tenant entity)
    {
        var document = TenantDocument.From(entity);
        await _repository.UpdateAsync(document);
    }
}
