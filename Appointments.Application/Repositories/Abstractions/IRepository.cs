using Appointments.Domain.Models;

namespace Appointments.Application.Repositories.Abstractions;

public interface IRepository<TEntity>
{
    Task<TEntity> CreateAsync(TEntity entity);
    Task<PagedResult<TEntity>> GetAsync(int pageIndex, int pageSize);
    Task<TEntity> GetByIdAsync(Guid id);
    Task<TEntity?> GetByIdOrDefaultAsync(Guid id);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(Guid id);
    Task PurgeAsync(Guid id);
}
