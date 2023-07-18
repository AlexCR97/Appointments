using Appointments.Domain.Entities;

namespace Appointments.Domain.Models;

public class TenantProfile
{
    public Guid Id { get; set; }
    public string UrlId { get; set; }
    public string? Logo { get; set; }
    public string Name { get; set; }
    public string? Slogan { get; set; }
    public List<SocialMediaContact> SocialMediaContacts { get; set; }
}
