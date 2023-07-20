using Appointments.Domain.Events.Tenants;
using Appointments.Domain.Models;

namespace Appointments.Domain.Entities;

public class Tenant : Entity
{
    public const int UrlIdLength = 8;

    public string Name { get; private set; }
    public string? Slogan { get; private set; }
    public string UrlId { get; private set; }
    public string? Logo { get; private set; }
    public List<SocialMediaContact> SocialMediaContacts { get; private set; }
    public WeeklySchedule? WeeklySchedule { get; private set; }

    public Tenant()
    {
        // Needed for auto-mapping
    }

    public Tenant(
        Guid id,
        DateTime createdAt,
        string? createdBy,
        DateTime? updatedAt,
        string? updatedBy,
        DateTime? deletedAt,
        string? deletedBy,
        List<string> tags,
        Dictionary<string, string?> extensions,

        string name,
        string? slogan,
        string urlId,
        string? logo,
        List<SocialMediaContact> socialMediaContacts,
        WeeklySchedule? weeklySchedule)
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
        Name = name;
        Slogan = slogan;
        UrlId = urlId;
        Logo = logo;
        SocialMediaContacts = socialMediaContacts;
        WeeklySchedule = weeklySchedule;
    }

    public static Tenant CreateMinimal(
        string? createdBy,
        string name,
        string urlId)
    {
        var tenant = new Tenant(
            Guid.NewGuid(),
            DateTime.UtcNow,
            createdBy,
            null,
            null,
            null,
            null,
            new List<string>(),
            new Dictionary<string, string?>(),

            name,
            null,
            urlId,
            null,
            new List<SocialMediaContact>(),
            WeeklySchedule.NineToFive);

        tenant.AddEvent(new MinimalTenantCreatedEvent(
            tenant.CreatedAt,
            createdBy,
            name,
            urlId));

        return tenant;
    }

    public TenantProfile GetProfile() => new(
        Id,
        UrlId,
        Logo,
        Name,
        Slogan,
        SocialMediaContacts);

    public void Update(
        string? updatedBy,
        string name,
        string? slogan,
        string urlId,
        string? logo,
        List<SocialMediaContact>? socialMediaContacts,
        WeeklySchedule? weeklySchedule)
    {
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;

        Name = name;
        Slogan = slogan;
        UrlId = urlId;
        Logo = logo;
        SocialMediaContacts = socialMediaContacts ?? new();
        WeeklySchedule = weeklySchedule;

        AddEvent(new TenantUpdateEvent(
            Id,
            UpdatedAt.Value,
            updatedBy,
            name,
            slogan,
            urlId,
            logo,
            socialMediaContacts,
            weeklySchedule));
    }

    public void Delete(string? deletedBy)
    {
        DeletedAt = DateTime.UtcNow;
        DeletedBy = deletedBy;

        AddEvent(new TenantDeletedEvent(
            DeletedAt.Value,
            deletedBy));
    }
}
