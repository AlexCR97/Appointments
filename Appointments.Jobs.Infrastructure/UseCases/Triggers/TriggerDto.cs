using Appointments.Common.Domain.Enums;
using Appointments.Jobs.Domain.Triggers;

namespace Appointments.Jobs.Infrastructure.UseCases.Triggers;

public sealed record TriggerDto(
    string Id,
    DateTime CreatedAt,
    string CreatedBy,
    DateTime? UpdatedAt,
    string? UpdatedBy,
    string Type,
    string Name,
    string? DisplayName);

public static class TriggerDtoExtensions
{
    public static TriggerDto ToDto(this Trigger trigger)
    {
        return new TriggerDto(
            trigger.Id.ToString(),
            trigger.CreatedAt,
            trigger.CreatedBy,
            trigger.UpdatedAt,
            trigger.UpdatedBy,
            trigger.Type.ToString(),
            trigger.Name.Value,
            trigger.DisplayName);
    }

    public static Trigger ToEntity(this TriggerDto trigger)
    {
        var triggerType = trigger.Type.ToEnum<TriggerType>();

        if (triggerType == TriggerType.FireAndForget)
        {
            return new FireAndForgetTrigger(
                Guid.Parse(trigger.Id),
                trigger.CreatedAt,
                trigger.CreatedBy,
                trigger.UpdatedAt,
                trigger.UpdatedBy,
                new TriggerName(trigger.Name),
                trigger.DisplayName);
        }

        throw new UnsupportedTriggerTypeException(triggerType);
    }
}
