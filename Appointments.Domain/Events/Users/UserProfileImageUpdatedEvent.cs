using Appointments.Domain.Events.Abstractions;

namespace Appointments.Domain.Events.Users;

public record UserProfileImageUpdatedEvent(
    Guid Id,
    DateTime UpdatedAt,
    string? UpdatedBy,
    string ProfileImage) : IEvent;
