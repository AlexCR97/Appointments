using Appointments.Common.Domain;

namespace Appointments.Core.Domain.Entities;

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
