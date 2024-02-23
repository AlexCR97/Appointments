namespace Appointments.Jobs.Infrastructure.UseCases.Jobs;

public sealed record JobCreatedEvent(
    Guid Id,
    DateTime OccurredAt,
    string CreatedBy,
    string Group,
    string Name,
    string? DisplayName);
