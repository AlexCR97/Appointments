using Appointments.Common.Domain.Exceptions;
using Appointments.Common.Domain.Models;
using Appointments.Common.MongoClient.Abstractions;
using Appointments.Core.Application.Requests.BranchOffices;
using Appointments.Core.Domain.Entities;

namespace Appointments.Core.Infrastructure.Mongo.BranchOffices;

internal sealed class BranchOfficeRepository : IBranchOfficeRepository
{
    private readonly IMongoRepository<BranchOfficeDocument> _repository;

    public BranchOfficeRepository(IMongoRepository<BranchOfficeDocument> repository)
    {
        _repository = repository;
    }

    public async Task<BranchOffice> CreateAsync(BranchOffice entity)
    {
        var document = BranchOfficeDocument.From(entity);
        var createdDocument = await _repository.CreateAsync(document);
        return createdDocument.ToEntity();
    }

    public async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<PagedResult<BranchOffice>> FindAsync(int pageIndex, int pageSize, string? sort, string? filter)
    {
        var documents = await _repository.GetAsync(
            pageIndex,
            pageSize,
            sort: sort,
            filter: filter);

        return new PagedResult<BranchOffice>(
            pageIndex,
            pageSize,
            documents.TotalCount,
            documents.Results
                .Select(x => x.ToEntity())
                .ToList());
    }

    public async Task<BranchOffice> GetAsync(Guid id)
    {
        try
        {
            var document = await _repository.GetOneAsync(id);
            return document.ToEntity();
        }
        catch (Exception ex)
        {
            if (ex is Common.MongoClient.Exceptions.NotFoundException<BranchOfficeDocument>)
                throw new NotFoundException(nameof(BranchOffice), "ID", id.ToString());

            throw;
        }
    }

    public async Task<BranchOffice?> GetOrDefaultAsync(Guid id)
    {
        var document = await _repository.GetOneOrDefaultAsync(id);
        return document?.ToEntity();
    }

    public async Task UpdateAsync(BranchOffice entity)
    {
        var document = BranchOfficeDocument.From(entity);
        await _repository.UpdateAsync(document);
    }
}
