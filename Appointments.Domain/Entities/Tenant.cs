namespace Appointments.Domain.Entities;

public class Tenant : Entity
{
    public string Name { get; set; }
    public string? Slogan { get; set; }
    public string UrlId { get; set; }
    public string? Logo { get; set; }
    public List<SocialMediaContact>? SocialMediaContacts { get; set; }
    public WeeklySchedule? WeeklySchedule { get; set; }
}
