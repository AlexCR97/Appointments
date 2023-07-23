namespace Appointments.Domain.Entities;

public class SocialMediaContact
{
    public string Contact { get; set; } = string.Empty;
    public SocialMediaType Type { get; set; }
    public string? OtherType { get; set; }
}
