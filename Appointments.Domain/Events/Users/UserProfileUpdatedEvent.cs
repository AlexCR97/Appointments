namespace Appointments.Domain.Events.Users;

public class UserProfileUpdatedEvent : Event
{
    public Guid Id { get; }
    public DateTime UpdatedAt { get; }
    public string? UpdatedBy { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string? ProfileImage { get; }

    public UserProfileUpdatedEvent(Guid id, DateTime updatedAt, string? updatedBy, string firstName, string lastName, string? profileImage)
    {
        Id = id;
        UpdatedAt = updatedAt;
        UpdatedBy = updatedBy;
        FirstName = firstName;
        LastName = lastName;
        ProfileImage = profileImage;
    }
}
