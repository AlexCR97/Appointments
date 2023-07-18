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

    public Tenant(
        string? createdBy,
        string name,
        string? slogan,
        string urlId,
        string? logo,
        List<SocialMediaContact>? socialMediaContacts,
        WeeklySchedule? weeklySchedule)
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        CreatedBy = createdBy;

        Name = name;
        Slogan = slogan;
        UrlId = urlId;
        Logo = logo;
        SocialMediaContacts = socialMediaContacts ?? new();
        WeeklySchedule = weeklySchedule;
    }

    public TenantProfile GetProfile()
    {
        return new TenantProfile(
            Id,
            UrlId,
            Logo,
            Name,
            Slogan,
            SocialMediaContacts);
    }

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
    }

    public void Delete(string? deletedBy)
    {
        DeletedAt = DateTime.UtcNow;
        DeletedBy = deletedBy;
    }
}
