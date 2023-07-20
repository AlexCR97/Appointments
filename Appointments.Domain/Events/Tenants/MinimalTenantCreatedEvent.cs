using Appointments.Domain.Events.Abstractions;

namespace Appointments.Domain.Events.Tenants;

public class MinimalTenantCreatedEvent : IEvent
{
    public DateTime CreatedAt { get; }
    public string? CreatedBy { get; }
    public string Name { get; }
    public string UrlId { get; }

    public MinimalTenantCreatedEvent(DateTime createdAt, string? createdBy, string name, string urlId)
    {
        CreatedAt = createdAt;
        CreatedBy = createdBy;
        Name = name;
        UrlId = urlId;
    }
}
