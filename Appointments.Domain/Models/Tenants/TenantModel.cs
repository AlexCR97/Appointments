using Appointments.Domain.Entities;

namespace Appointments.Domain.Models.Tenants;

public class TenantModel
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }

    public string Name { get; set; } = string.Empty;
    public string? Slogan { get; set; }
    public string UrlId { get; set; } = string.Empty;
    public string? Logo { get; set; }
    public List<SocialMediaContact> SocialMediaContacts { get; set; } = new();
    public WeeklySchedule? WeeklySchedule { get; set; }
}
