using Appointments.Common.Domain.Exceptions;
using Appointments.Common.Domain.Models;
using Appointments.Common.MongoClient.Abstractions;
using Appointments.Jobs.Application.UseCases.Executions;
using Appointments.Jobs.Domain.Executions;

namespace Appointments.Jobs.Infrastructure.Mongo.Executions;

internal sealed class ExecutionRepository : IExecutionRepository
{
    private readonly IMongoRepository<ExecutionDocument> _repository;

    public ExecutionRepository(IMongoRepository<ExecutionDocument> repository)
    {
        _repository = repository;
    }

    public async Task<Execution> CreateAsync(Execution entity)
    {
        var document = ExecutionDocument.From(entity);
        var createdDocument = await _repository.CreateAsync(document);
        return createdDocument.ToEntity();
    }

    public async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<PagedResult<Execution>> FindAsync(int pageIndex, int pageSize, string? sort, string? filter)
    {
        var documents = await _repository.GetAsync(
            pageIndex,
            pageSize,
            sort: sort,
            filter: filter);

        return new PagedResult<Execution>(
            pageIndex,
            pageSize,
            documents.TotalCount,
            documents.Results
                .Select(x => x.ToEntity())
                .ToList());
    }

    public async Task<Execution> GetAsync(Guid id)
    {
        try
        {
            var document = await _repository.GetOneAsync(id);
            return document.ToEntity();
        }
        catch (Exception ex)
        {
            if (ex is Common.MongoClient.Exceptions.NotFoundException<ExecutionDocument>)
                throw new NotFoundException(nameof(Execution), "ID", id.ToString());

            throw;
        }
    }

    public async Task<Execution?> GetOrDefaultAsync(Guid id)
    {
        var document = await _repository.GetOneOrDefaultAsync(id);
        return document?.ToEntity();
    }

    public async Task UpdateAsync(Execution entity)
    {
        var document = ExecutionDocument.From(entity);
        await _repository.UpdateAsync(document);
    }
}
