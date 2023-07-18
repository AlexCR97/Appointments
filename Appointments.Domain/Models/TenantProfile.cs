using Appointments.Domain.Entities;

namespace Appointments.Domain.Models;

public class TenantProfile
{
    public Guid Id { get; }
    public string UrlId { get; }
    public string? Logo { get; }
    public string Name { get; }
    public string? Slogan { get; }
    public List<SocialMediaContact> SocialMediaContacts { get; }

    public TenantProfile(Guid id, string urlId, string? logo, string name, string? slogan, List<SocialMediaContact> socialMediaContacts)
    {
        Id = id;
        UrlId = urlId;
        Logo = logo;
        Name = name;
        Slogan = slogan;
        SocialMediaContacts = socialMediaContacts;
    }
}
