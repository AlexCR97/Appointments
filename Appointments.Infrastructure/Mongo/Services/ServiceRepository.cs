using Appointments.Common.Domain.Exceptions;
using Appointments.Common.Domain.Models;
using Appointments.Common.MongoClient.Abstractions;
using Appointments.Core.Application.Requests.Services;
using Appointments.Core.Domain.Entities;

namespace Appointments.Core.Infrastructure.Mongo.Services;

internal sealed class ServiceRepository : IServiceRepository
{
    private readonly IMongoRepository<ServiceDocument> _repository;

    public ServiceRepository(IMongoRepository<ServiceDocument> repository)
    {
        _repository = repository;
    }

    public async Task<Service> CreateAsync(Service entity)
    {
        var document = ServiceDocument.From(entity);
        var createdDocument = await _repository.CreateAsync(document);
        return createdDocument.ToEntity();
    }

    public async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<PagedResult<Service>> FindAsync(int pageIndex, int pageSize, string? sort, string? filter)
    {
        var documents = await _repository.GetAsync(
            pageIndex,
            pageSize,
            sort: sort,
            filter: filter);

        return new PagedResult<Service>(
            pageIndex,
            pageSize,
            documents.TotalCount,
            documents.Results
                .Select(x => x.ToEntity())
                .ToList());
    }

    public async Task<Service> GetAsync(Guid id)
    {
        try
        {
            var document = await _repository.GetOneAsync(id);
            return document.ToEntity();
        }
        catch (Exception ex)
        {
            if (ex is Common.MongoClient.Exceptions.NotFoundException<ServiceDocument>)
                throw new NotFoundException(nameof(Service), "ID", id.ToString());

            throw;
        }
    }

    public async Task<Service?> GetOrDefaultAsync(Guid id)
    {
        var document = await _repository.GetOneOrDefaultAsync(id);
        return document?.ToEntity();
    }

    public async Task UpdateAsync(Service entity)
    {
        var document = ServiceDocument.From(entity);
        await _repository.UpdateAsync(document);
    }
}
