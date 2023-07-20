using Appointments.Domain.Events.Users;

namespace Appointments.Domain.Entities;

public class User : Entity
{
    public string Email { get; private set; }
    public string Password { get; private set; }
    public bool IsPasswordPlainText { get; private set; }
    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }
    public string? ProfileImage { get; private set; }

    public User()
    {
        // Needed for auto-mapping
    }

    public User(
        Guid id,
        DateTime createdAt,
        string? createdBy,
        DateTime? updatedAt,
        string? updatedBy,
        DateTime? deletedAt,
        string? deletedBy,
        List<string> tags,
        Dictionary<string, string?> extensions,
        
        string email,
        string password,
        bool isPasswordPlainText,
        string? firstName,
        string? lastName,
        string? profileImage)
    : base(
        id,
        createdAt,
        createdBy,
        updatedAt,
        updatedBy,
        deletedAt,
        deletedBy,
        tags,
        extensions)
    {
        Email = email;
        Password = password;
        IsPasswordPlainText = isPasswordPlainText;
        FirstName = firstName;
        LastName = lastName;
        ProfileImage = profileImage;
    }

    public static User CreateWithEmailCredentials(
        Guid id,
        string? createdBy,
        string email,
        string password,
        bool isPasswordPlainText,
        string? firstName,
        string? lastName,
        string? profileImage)
    {
        var user = new User(
            id,
            DateTime.UtcNow,
            createdBy,
            null,
            null,
            null,
            null,
            new List<string>(),
            new Dictionary<string, string?>(),

            email,
            password,
            isPasswordPlainText,
            firstName,
            lastName,
            profileImage);

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
