using Appointments.Domain.Events.Users;

namespace Appointments.Domain.Entities;

public class User : Entity
{
    public string Email { get; private set; } = null!;
    public string Password { get; private set; } = null!;
    public bool IsPasswordPlainText { get; private set; }
    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }
    public string? ProfileImage { get; private set; }

    private User() { }

    public static User CreateWithEmailCredentials(
        string? createdBy,
        string email,
        string password,
        bool isPasswordPlainText,
        string? firstName,
        string? lastName,
        string? profileImage)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            CreatedBy = createdBy,

            Email = email,
            Password = password,
            IsPasswordPlainText = isPasswordPlainText,
            FirstName = firstName,
            LastName = lastName,
            ProfileImage = profileImage,
        };

        user.AddEvent(new UserCreatedWithEmailEvent(
            user.Id,
            user.CreatedAt,
            user.CreatedBy,
            user.Email,
            user.Password,
            user.IsPasswordPlainText,
            user.FirstName,
            user.LastName,
            user.ProfileImage));

        return user;
    }

    public void UpdateProfile(
        string? updatedBy,
        string firstName,
        string lastName,
        string? profileImage)
    {
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;

        FirstName = firstName;
        LastName = lastName;
        ProfileImage = profileImage;

        AddEvent(new UserProfileUpdatedEvent(
            Id,
            UpdatedAt.Value,
            updatedBy,
            firstName,
            lastName,
            profileImage));
    }

    public void SetSelectedTenant(Tenant tenant, string? updatedBy)
    {
        SetExtension("SelectedTenant", tenant.Id.ToString(), updatedBy);
    }

    public void Delete(string? deletedBy)
    {
        DeletedAt = DateTime.UtcNow;
        DeletedBy = deletedBy;

        AddEvent(new UserDeletedEvent(
            Id,
            DeletedAt.Value,
            deletedBy));
    }
}
