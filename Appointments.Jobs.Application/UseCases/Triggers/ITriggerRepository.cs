using Appointments.Common.Application;
using Appointments.Jobs.Domain.Triggers;

namespace Appointments.Jobs.Application.UseCases.Triggers;

public interface ITriggerRepository : IRepository<Trigger>
{
    Task<Trigger> GetByTypeAsync(TriggerType type);
}
