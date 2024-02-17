using Appointments.Common.Domain.Exceptions;
using Appointments.Common.Domain.Models;
using Appointments.Common.MongoClient.Abstractions;
using Appointments.Jobs.Application.UseCases.Jobs;
using Appointments.Jobs.Domain.Jobs;

namespace Appointments.Jobs.Infrastructure.Mongo.Jobs;

internal sealed class JobRepository : IJobRepository
{
    private readonly IMongoRepository<JobDocument> _repository;

    public JobRepository(IMongoRepository<JobDocument> repository)
    {
        _repository = repository;
    }

    public async Task<Job> CreateAsync(Job entity)
    {
        throw new NotImplementedException();

        //var document = JobDocument.From(entity);
        //var createdDocument = await _repository.CreateAsync(document);
        //return createdDocument.ToEntity();
    }

    public async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<PagedResult<Job>> FindAsync(int pageIndex, int pageSize, string? sort, string? filter)
    {
        throw new NotImplementedException();

        //var documents = await _repository.GetAsync(
        //    pageIndex,
        //    pageSize,
        //    sort: sort,
        //    filter: filter);

        //return new PagedResult<Job>(
        //    pageIndex,
        //    pageSize,
        //    documents.TotalCount,
        //    documents.Results
        //        .Select(x => x.ToEntity())
        //        .ToList());
    }

    public async Task<Job> GetAsync(Guid id)
    {
        throw new NotImplementedException();

        //try
        //{
        //    var document = await _repository.GetOneAsync(id);
        //    return document.ToEntity();
        //}
        //catch (Exception ex)
        //{
        //    if (ex is Common.MongoClient.Exceptions.NotFoundException<JobDocument>)
        //        throw new NotFoundException(nameof(Job), "ID", id.ToString());

        //    throw;
        //}
    }

    public async Task<Job?> GetOrDefaultAsync(Guid id)
    {
        throw new NotImplementedException();

        //var document = await _repository.GetOneOrDefaultAsync(id);
        //return document?.ToEntity();
    }

    public async Task UpdateAsync(Job entity)
    {
        throw new NotImplementedException();

        //var document = JobDocument.From(entity);
        //await _repository.UpdateAsync(document);
    }
}
