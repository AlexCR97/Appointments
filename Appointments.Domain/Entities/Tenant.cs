using System.Security.Cryptography;

namespace Appointments.Domain.Entities;

public sealed class Tenant : Entity
{
    public string Name { get; private set; }
    public string? Slogan { get; private set; }
    public TenantUrlId UrlId { get; private set; }
    public string? Logo { get; private set; }

    private readonly List<SocialMediaContact> _contacts = new();
    public IReadOnlyList<SocialMediaContact> Contacts
    {
        get
        {
            return _contacts;
        }
        private set
        {
            _contacts.Clear();
            _contacts.AddRange(value);
        }
    }

    public WeeklySchedule? Schedule { get; private set; }

    public Tenant(
        Guid id,
        DateTime createdAt,
        string createdBy,
        DateTime? updatedAt,
        string? updatedBy,
        string name,
        string? slogan,
        TenantUrlId urlId,
        string? logo,
        IReadOnlyList<SocialMediaContact> socialMediaContacts,
        WeeklySchedule? weeklySchedule)
    : base(
        id,
        createdAt,
        createdBy,
        updatedAt,
        updatedBy)
    {
        Name = name;
        Slogan = slogan;
        UrlId = urlId;
        Logo = logo;
        Contacts = socialMediaContacts;
        Schedule = weeklySchedule;
    }

    public void SetSchedule(
        string updatedBy,
        WeeklySchedule? schedule)
    {
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
        Schedule = schedule;

        // TODO Add event
    }

    public void SetLogo(
        string updatedBy,
        string? logo)
    {
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
        Logo = logo;

        // TODO Add event
    }

    public void UpdateProfile(
        string updatedBy,
        string name,
        string? slogan,
        TenantUrlId urlId,
        IReadOnlyList<SocialMediaContact> contacts)
    {
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
        Name = name;
        Slogan = slogan;
        UrlId = urlId;
        Contacts = contacts;

        // TODO Add event
    }

    public static Tenant Create(
        string createdBy,
        string name,
        string? slogan,
        TenantUrlId urlId,
        string? logo,
        IReadOnlyList<SocialMediaContact> socialMediaContacts,
        WeeklySchedule? weeklySchedule)
    {
        var tenant = new Tenant(
            Guid.NewGuid(),
            DateTime.UtcNow,
            createdBy,
            null,
            null,
            name,
            slogan,
            urlId,
            logo,
            socialMediaContacts,
            weeklySchedule);

        // TODO Add event

        return tenant;
    }
}

public readonly struct TenantUrlId
{
    public const int MinLength = 1;
    public const int DefaultLength = 8;
    public const int MaxLength = 32;

    public readonly string Value;

    public TenantUrlId()
    {
        Value = string.Empty;
    }

    public TenantUrlId(string value)
    {
        Value = value;
    }

    public override string ToString()
    {
        return Value;
    }

    public static TenantUrlId Random()
    {
        var randomKey = GenerateRandomKey(DefaultLength);
        return new TenantUrlId(randomKey);
    }

    private static string GenerateRandomKey(int length)
    {
        const string charset = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        var bytes = new byte[length];

        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);

        var result = new char[length];
        var cursor = 0;

        for (var i = 0; i < length; i++)
        {
            cursor += bytes[i];
            result[i] = charset[cursor % charset.Length];
        }

        return new string(result);
    }
}

#region Events

public sealed record TenantCreatedEvent(
    Guid Id,
    DateTime CreatedAt,
    string CreatedBy,
    string Name,
    string? Slogan,
    TenantUrlId UrlId,
    string? Logo,
    IReadOnlyList<SocialMediaContact> SocialMediaContacts,
    WeeklySchedule? WeeklySchedule)
    : IDomainEvent;

public sealed record TenantUpdatedEvent(
    Guid Id,
    DateTime UpdatedAt,
    string UpdatedBy,
    string Name,
    string? Slogan,
    TenantUrlId UrlId,
    IReadOnlyList<SocialMediaContact> SocialMediaContacts,
    WeeklySchedule? WeeklySchedule)
    : IDomainEvent;

public sealed record TenantDeletedEvent(
    DateTime DeletedAt,
    string DeletedBy)
    : IDomainEvent;

#endregion
