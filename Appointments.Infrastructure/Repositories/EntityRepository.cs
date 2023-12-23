using Appointments.Application.Mapper.Abstractions;
using Appointments.Application.Requests;
using Appointments.Common.MongoClient.Abstractions;
using Appointments.Domain.Entities;
using Appointments.Domain.Models;

namespace Appointments.Infrastructure.Repositories;

internal interface IEntityRepository<TEntity, TDocument> : IRepository<TEntity>
    where TEntity : Entity
    where TDocument : IMongoDocument
{
}

internal class EntityRepository<TEntity, TDocument> : IEntityRepository<TEntity, TDocument>
    where TEntity : Entity
    where TDocument : IMongoDocument
{
    protected readonly IMapper _mapper;
    protected readonly IMongoRepository<TDocument> _repository;

    public EntityRepository(IMapper mapper, IMongoRepository<TDocument> repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<TEntity> CreateAsync(TEntity entity)
    {
        var document = _mapper.Map<TDocument>(entity);
        var createdDocument = await _repository.CreateAsync(document);
        return _mapper.Map<TEntity>(createdDocument);
    }

    public async Task<PagedResult<TEntity>> FindAsync(int pageIndex, int pageSize, string? sort, string? filter)
    {
        var documents = await _repository.GetAsync(
            pageIndex,
            pageSize,
            sort: sort,
            filter: filter);

        return new PagedResult<TEntity>(
            pageIndex,
            pageSize,
            documents.TotalCount,
            documents.Results
                .Select(x => _mapper.Map<TEntity>(x))
                .ToList());
    }

    public async Task<TEntity> GetAsync(Guid id)
    {
        try
        {
            var document = await _repository.GetOneAsync(id);
            return _mapper.Map<TEntity>(document);
        }
        catch (Exception ex)
        {
            if (ex is Common.MongoClient.Exceptions.NotFoundException<TDocument>)
                throw new Application.Exceptions.NotFoundException(typeof(TEntity).Name, id);

            throw;
        }
    }

    public async Task<TEntity?> GetOrDefaultAsync(Guid id)
    {
        var document = await _repository.GetOneOrDefaultAsync(id);
        
        return document is null
            ? null
            : _mapper.Map<TEntity>(document);
    }

    public async Task UpdateAsync(TEntity entity)
    {
        var document = _mapper.Map<TDocument>(entity);
        await _repository.UpdateAsync(document);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task PurgeAsync(Guid id)
    {
        await _repository.PurgeAsync(id);
    }
}
