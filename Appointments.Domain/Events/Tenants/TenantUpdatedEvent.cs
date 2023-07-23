using Appointments.Domain.Entities;
using Appointments.Domain.Events.Abstractions;

namespace Appointments.Domain.Events.Tenants;

internal sealed record TenantUpdatedEvent(
    Guid Id,
    DateTime UpdatedAt,
    string? UpdatedBy,
    string Name,
    string? Slogan,
    string UrlId,
    List<SocialMediaContact>? SocialMediaContacts) : IEvent;
