using Appointments.Common.Domain;

namespace Appointments.Core.Application.Requests.Users;

public sealed record UserSignedUpWithEmailEvent(
    Guid Id,
    DateTime OccurredAt,
    Guid UserId)
    : IDomainEvent;
