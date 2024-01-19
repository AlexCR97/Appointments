using Appointments.Core.Domain.Entities;

namespace Appointments.Infrastructure.Mongo.Documents;

internal sealed record SocialMediaContactDocument(
    string Type,
    string? OtherType,
    string Contact)
{
    internal static SocialMediaContactDocument From(SocialMediaContact contact)
    {
        return new SocialMediaContactDocument(
            contact.Type.ToString(),
            contact.OtherType,
            contact.Contact);
    }

    internal SocialMediaContact ToEntity()
    {
        return new SocialMediaContact(
            Enum.Parse<SocialMediaType>(Type),
            OtherType,
            Contact);
    }
}
