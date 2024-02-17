using Appointments.Common.Domain.Models;
using Appointments.Jobs.Application.UseCases.Triggers;
using Appointments.Jobs.Domain.Triggers;

namespace Appointments.Jobs.Infrastructure.Mongo.Triggers;

internal sealed class TriggerRepository : ITriggerRepository
{
    public Task<Trigger> CreateAsync(Trigger entity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<PagedResult<Trigger>> FindAsync(int pageIndex, int pageSize, string? sort, string? filter)
    {
        throw new NotImplementedException();
    }

    public Task<Trigger> GetAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Trigger> GetByTypeAsync(TriggerType type)
    {
        throw new NotImplementedException();
    }

    public Task<Trigger?> GetOrDefaultAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Trigger entity)
    {
        throw new NotImplementedException();
    }
}
