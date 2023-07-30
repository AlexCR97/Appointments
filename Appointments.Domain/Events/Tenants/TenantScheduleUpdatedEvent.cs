using Appointments.Domain.Entities;
using Appointments.Domain.Events.Abstractions;

namespace Appointments.Domain.Events.Tenants;

internal sealed record TenantScheduleUpdatedEvent(
    Guid Id,
    DateTime UpdatedAt,
    string? UpdatedBy,
    WeeklySchedule WeeklySchedule) : IEvent;
