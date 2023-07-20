using Appointments.Application.Repositories.Abstractions;
using Appointments.Common.MongoClient.Abstractions;
using Appointments.Domain.Entities;
using Appointments.Domain.Models;
using Appointments.Infrastructure.Mapper.Abstractions;

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

    public Task<PagedResult<TEntity>> GetAsync(int pageIndex, int pageSize)
    {
        throw new NotImplementedException();
    }

    public async Task<TEntity> GetByIdAsync(Guid id)
    {
        var document = await _repository.GetOneAsync(id);
        return _mapper.Map<TEntity>(document);
    }

    public Task<TEntity?> GetByIdOrDefaultAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task PurgeAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}
