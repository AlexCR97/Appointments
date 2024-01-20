using Appointments.Common.Domain;
using Appointments.Common.Domain.Exceptions;
using Appointments.Common.Domain.Models;

namespace Appointments.Core.Domain.Entities;

public sealed class User : Entity
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string? ProfileImage { get; private set; }

    private readonly List<IUserLogin> _logins = new();
    public IReadOnlyList<IUserLogin> Logins
    {
        get
        {
            return _logins;
        }
        private set
        {
            _logins.Clear();
            _logins.AddRange(value);
        }
    }

    private readonly List<UserTenant> _tenants = new();
    public IReadOnlyList<UserTenant> Tenants
    {
        get
        {
            return _tenants;
        }
        private set
        {
            _tenants.Clear();
            _tenants.AddRange(value);
        }
    }

    private readonly List<UserPreference> _preferences = new();
    public IReadOnlyList<UserPreference> Preferences
    {
        get
        {
            return _preferences;
        }
        private set
        {
            _preferences.Clear();
            _preferences.AddRange(value);
        }
    }

    public string FullName => $"{FirstName} {LastName}";

    public User(
        Guid id,
        DateTime createdAt,
        string createdBy,
        DateTime? updatedAt,
        string? updatedBy,
        string firstName,
        string lastName,
        string? profileImage,
        IReadOnlyList<IUserLogin> logins,
        IReadOnlyList<UserTenant> tenants,
        IReadOnlyList<UserPreference> preferences)
    : base(
        id,
        createdAt,
        createdBy,
        updatedAt,
        updatedBy)
    {
        FirstName = firstName;
        LastName = lastName;
        ProfileImage = profileImage;
        Logins = logins;
        Tenants = tenants;
        Preferences = preferences;
    }

    public void ConfirmLogin(string updatedBy, IUserLogin login)
    {
        login.Confirm();
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;

        // TODO Add UserLoginConfirmedEvent
    }

    public LocalLogin GetLocalLogin()
    {
        var localLogin = Logins.FirstOrDefault(x => x.IdentityProvider == IdentityProvider.Local)
            ?? throw new OwnershipException("User", Id.ToString(), "Login", IdentityProvider.Local.ToString());

        return (LocalLogin)localLogin;
    }

    public UserTenant GetTenant(Guid tenantId)
    {
        return GetTenantOrDefault(tenantId)
            ?? throw new OwnershipException("User", Id.ToString(), "Tenant", tenantId.ToString());
    }

    public UserTenant? GetTenantOrDefault(Guid tenantId)
    {
        return Tenants.FirstOrDefault(x => x.TenantId == tenantId);
    }

    public void UpdateProfile(
        string updatedBy,
        string firstName,
        string lastName)
    {
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
        FirstName = firstName;
        LastName = lastName;

        // TODO Add event
    }

    public void UpdateProfileImage(
        string updatedBy,
        string profileImage)
    {
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
        ProfileImage = profileImage;

        // TODO Add event
    }

    public static User Create(
        string createdBy,
        string firstName,
        string lastName,
        string? profileImage,
        IReadOnlyList<LocalLogin> logins,
        IReadOnlyList<UserTenant> tenants,
        IReadOnlyList<UserPreference> preferences)
    {
        var user = new User(
            Guid.NewGuid(),
            DateTime.UtcNow,
            createdBy,
            null,
            null,
            firstName,
            lastName,
            profileImage,
            logins,
            tenants,
            preferences);

        // TODO Add event

        return user;
    }

    public static User CreateWithLocalLogin(
        string createdBy,
        string firstName,
        string lastName,
        Email email,
        string password,
        UserTenant tenant)
    {
        return Create(
            createdBy,
            firstName,
            lastName,
            null,
            new List<LocalLogin> { LocalLogin.Create(email, password) },
            new List<UserTenant> { tenant },
            new List<UserPreference> { UserPreference.CreateSelectedTenantPreference(tenant.TenantId) });
    }
}

public sealed record UserTenant(
    Guid TenantId,
    string TenantName);

public struct UserPreference
{
    public string Key { get; }
    public string Value { get; private set; }

    public UserPreference()
    {
        Key = string.Empty;
        Value = string.Empty;
    }

    public UserPreference(string key, string value)
    {
        Key = key;
        Value = value;
    }

    public void Update(string value)
    {
        Value = value;
    }

    public static class CommonCodes
    {
        public const string SelectedTenant = "selectedTenant";
    }

    public static UserPreference CreateSelectedTenantPreference(Guid tenantId)
    {
        return new UserPreference(CommonCodes.SelectedTenant, tenantId.ToString());
    }
}
