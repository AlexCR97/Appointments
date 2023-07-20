using Appointments.Domain.Entities;
using Appointments.Domain.Events.Abstractions;

namespace Appointments.Domain.Events.Tenants;
internal class TenantUpdateEvent : IEvent
{
    public Guid Id { get; }
    public DateTime UpdatedAt { get; }
    public string? UpdatedBy { get; }
    public string Name { get; }
    public string? Slogan { get; }
    public string UrlId { get; }
    public string? Logo { get; }
    public List<SocialMediaContact>? SocialMediaContacts { get; }
    public WeeklySchedule? WeeklySchedule { get; }

    public TenantUpdateEvent(Guid id, DateTime updatedAt, string? updatedBy, string name, string? slogan, string urlId, string? logo, List<SocialMediaContact>? socialMediaContacts, WeeklySchedule? weeklySchedule)
    {
        Id = id;
        UpdatedAt = updatedAt;
        UpdatedBy = updatedBy;
        Name = name;
        Slogan = slogan;
        UrlId = urlId;
        Logo = logo;
        SocialMediaContacts = socialMediaContacts;
        WeeklySchedule = weeklySchedule;
    }
}
