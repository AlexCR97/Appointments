using Appointments.Common.Domain.Models;
using Appointments.Jobs.Application.UseCases.Executions;
using Appointments.Jobs.Domain;

namespace Appointments.Jobs.Infrastructure.Mongo.Executions;

internal sealed class ExecutionRepository : IExecutionRepository
{
    public Task<Execution> CreateAsync(Execution entity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<PagedResult<Execution>> FindAsync(int pageIndex, int pageSize, string? sort, string? filter)
    {
        throw new NotImplementedException();
    }

    public Task<Execution> GetAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Execution?> GetOrDefaultAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Execution entity)
    {
        throw new NotImplementedException();
    }
}
