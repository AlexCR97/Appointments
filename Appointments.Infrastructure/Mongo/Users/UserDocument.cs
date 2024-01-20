using Appointments.Common.Utils.Exceptions;
using Appointments.Core.Domain.Entities;
using Appointments.Infrastructure.Mongo.Documents;
using MongoDB.Bson.Serialization.Attributes;

namespace Appointments.Core.Infrastructure.Mongo.Users;

internal sealed class UserDocument : MongoDocument
{
    public const string CollectionName = "users";

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? ProfileImage { get; set; }
    public UserLoginDocument[] Logins { get; set; }
    public UserTenantDocument[] Tenants { get; set; }
    public UserPreferenceDocument[] Preferences { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public UserDocument()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        // Required by Mongo Client library
    }

    public UserDocument(
        Guid id, DateTime createdAt, string createdBy, DateTime? updatedAt, string? updatedBy,
        string firstName, string lastName, string? profileImage, UserLoginDocument[] logins, UserTenantDocument[] tenants, UserPreferenceDocument[] preferences)
        : base(id, createdAt, createdBy, updatedAt, updatedBy)
    {
        FirstName = firstName;
        LastName = lastName;
        ProfileImage = profileImage;
        Logins = logins;
        Tenants = tenants;
        Preferences = preferences;
    }

    internal static UserDocument From(User entity)
    {
        return new UserDocument(
            entity.Id,
            entity.CreatedAt,
            entity.CreatedBy,
            entity.UpdatedAt,
            entity.UpdatedBy,
            entity.FirstName,
            entity.LastName,
            entity.ProfileImage,
            entity.Logins.Select(UserLoginDocument.From).ToArray(),
            entity.Tenants.Select(UserTenantDocument.From).ToArray(),
            entity.Preferences.Select(UserPreferenceDocument.From).ToArray());
    }

    internal User ToEntity()
    {
        return new User(
            Id,
            CreatedAt,
            CreatedBy,
            UpdatedAt,
            UpdatedBy,
            FirstName,
            LastName,
            ProfileImage,
            Logins.Select(x => x.ToEntity()).ToList(),
            Tenants.Select(x => x.ToEntity()).ToList(),
            Preferences.Select(x => x.ToEntity()).ToList());
    }
}

internal sealed record UserLoginDocument(
    string IdentityProvider,
    bool Confirmed,
    DateTime? ConfirmedAt,
    string Email,
    string Password,
    string ConfirmationCode,
    DateTime ConfirmationCodeExpiration)
{
    internal static UserLoginDocument From(IUserLogin login)
    {
        if (login is LocalLogin localLogin)
            return From(localLogin);

        throw new MappingExtension(login.GetType(), typeof(UserLoginDocument));
    }

    private static UserLoginDocument From(LocalLogin login)
    {
        return new UserLoginDocument(
            login.IdentityProvider.ToString(),
            login.Confirmed,
            login.ConfirmedAt,
            login.Email.Value,
            login.Password,
            login.ConfirmationCode,
            login.ConfirmationCodeExpiration);
    }

    internal LocalLogin ToEntity()
    {
        if (IdentityProvider == Domain.Entities.IdentityProvider.Local.ToString())
        {
            return new LocalLogin(
                Confirmed,
                ConfirmedAt,
                new Common.Domain.Models.Email(Email),
                Password,
                ConfirmationCode,
                ConfirmationCodeExpiration);
        }

        throw new MappingExtension(typeof(UserLoginDocument), typeof(IUserLogin));
    }
}

internal sealed record UserTenantDocument
{
    [BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public Guid TenantId { get; init; }
    public string TenantName { get; init; }

    public UserTenantDocument(Guid tenantId, string tenantName)
    {
        TenantId = tenantId;
        TenantName = tenantName;
    }

    internal static UserTenantDocument From(UserTenant tenant)
    {
        return new UserTenantDocument(
            tenant.TenantId,
            tenant.TenantName);
    }

    internal UserTenant ToEntity()
    {
        return new UserTenant(
            TenantId,
            TenantName);
    }
}

internal sealed record UserPreferenceDocument(
    string Key,
    string Value)
{
    internal static UserPreferenceDocument From(UserPreference preference)
    {
        return new UserPreferenceDocument(
            preference.Key,
            preference.Value);
    }

    internal UserPreference ToEntity()
    {
        return new UserPreference(
            Key,
            Value);
    }
}
