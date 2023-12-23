namespace Appointments.Domain.Entities;

public sealed record SocialMediaContact(
    SocialMediaType Type,
    string? OtherType,
    string Contact);
