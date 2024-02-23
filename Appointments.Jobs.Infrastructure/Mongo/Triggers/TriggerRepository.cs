using Appointments.Common.Domain.Exceptions;
using Appointments.Common.Domain.Models;
using Appointments.Jobs.Application.UseCases.Triggers;
using Appointments.Jobs.Domain.Triggers;

namespace Appointments.Jobs.Infrastructure.Mongo.Triggers;

internal sealed class TriggerRepository : ITriggerRepository
{
    private const string _createdBy = "Appointments.Jobs.Infrastructure";

    private static readonly IReadOnlyList<Trigger> _triggers = new List<Trigger>
    {
        new FireAndForgetTrigger(
            Guid.Parse("d9577541-ca48-4d10-b945-fb9a7446449a"),
            DateTime.UtcNow,
            _createdBy,
            null,
            null,
            new TriggerName("FireAndForget"),
            "Fire & Forget"),
    };

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
        var trigger = _triggers.FirstOrDefault(x => x.Type == type)
            ?? throw new NotFoundException(nameof(Trigger), nameof(Trigger.Type), type.ToString());

        return Task.FromResult(trigger);
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
