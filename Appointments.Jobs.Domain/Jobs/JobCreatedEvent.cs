using Appointments.Common.Domain;

namespace Appointments.Jobs.Domain.Jobs;

public sealed record JobCreatedEvent(
    Guid Id,
    DateTime OccurredAt,
    string CreatedBy,
    JobGroup Group,
    JobName Name,
    string? DisplayName)
    : IDomainEvent;
