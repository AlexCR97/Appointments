using Appointments.Domain.Events.Abstractions;

namespace Appointments.Domain.Events.Users;

public class UserDeletedEvent : IEvent
{
    public Guid Id { get; }
    public DateTime DeletedAt { get; }
    public string? DeletedBy { get; }

    public UserDeletedEvent(Guid id, DateTime deletedAt, string? deletedBy)
    {
        Id = id;
        DeletedAt = deletedAt;
        DeletedBy = deletedBy;
    }
}
