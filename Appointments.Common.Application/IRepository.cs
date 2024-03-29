﻿using Appointments.Common.Domain.Models;

namespace Appointments.Common.Application;

public interface IRepository<TEntity>
{
    Task<TEntity> CreateAsync(TEntity entity);
    Task<PagedResult<TEntity>> FindAsync(int pageIndex, int pageSize, string? sort, string? filter);
    Task<TEntity> GetAsync(Guid id);
    Task<TEntity?> GetOrDefaultAsync(Guid id);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(Guid id);
}
