using Appointments.Domain.Entities;
using Appointments.Domain.Events.Abstractions;

namespace Appointments.Domain.Events.Tenants;

public sealed record TenantCreatedEvent(
    Guid Id,
    DateTime CreatedAt,
    string? CreatedBy,
    string Name,
    string? Slogan,
    string UrlId,
    string? Logo,
    List<SocialMediaContact> SocialMediaContacts,
    WeeklySchedule? WeeklySchedule) : IEvent;
