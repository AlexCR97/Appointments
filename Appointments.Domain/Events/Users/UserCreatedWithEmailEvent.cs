namespace Appointments.Domain.Events.Users;

public class UserCreatedWithEmailEvent : Event
{
    public Guid Id { get; }
    public DateTime CreatedAt { get; }
    public string? CreatedBy { get; }
    public string Email { get; }
    public string Password { get; }
    public bool IsPasswordPlainText { get; }
    public string? FirstName { get; }
    public string? LastName { get; }
    public string? ProfileImage { get; }

    public UserCreatedWithEmailEvent(Guid id, DateTime createdAt, string? createdBy, string email, string password, bool isPasswordPlainText, string? firstName, string? lastName, string? profileImage)
    {
        Id = id;
        CreatedAt = createdAt;
        CreatedBy = createdBy;
        Email = email;
        Password = password;
        IsPasswordPlainText = isPasswordPlainText;
        FirstName = firstName;
        LastName = lastName;
        ProfileImage = profileImage;
    }
}
