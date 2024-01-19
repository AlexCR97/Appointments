using Appointments.Common.Domain.Enums;
using Appointments.Core.Domain.Entities;

namespace Appointments.Api.Tenant.Models;

public sealed record SocialMediaContactModel(
    string Type,
    string? OtherType,
    string Contact)
{
    internal static SocialMediaContactModel From(SocialMediaContact contact)
    {
        return new SocialMediaContactModel(
            contact.Type.ToString(),
            contact.OtherType,
            contact.Contact);
    }

    internal SocialMediaContact ToModel()
    {
        return new SocialMediaContact(
            Type.ToEnum<SocialMediaType>(),
            OtherType,
            Contact);
    }
}
