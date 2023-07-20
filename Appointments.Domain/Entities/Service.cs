using Appointments.Domain.Events.Services;

namespace Appointments.Domain.Entities;

public class Service : Entity
{
    public Guid TenantId { get; private set; }
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public string? Description { get; private set; }
    public string? Notes { get; private set; }

    /// <summary>
    /// The amount of time it takes for the customer to receive the service.
    /// </summary>
    public TimeSpan? CustomerDuration { get; private set; }

    /// <summary>
    /// The amount of time the service occupies in the calendar.
    /// </summary>
    public TimeSpan? CalendarDuration { get; private set; }
    
    public List<IndexedImage> Images { get; private set; }
    public List<string> TermsAndConditions { get; private set; }

    public Service(
        Guid id,
        DateTime createdAt,
        string? createdBy,
        DateTime? updatedAt,
        string? updatedBy,
        DateTime? deletedAt,
        string? deletedBy,
        List<string> tags,
        Dictionary<string, string?> extensions,

        Guid tenantId,
        string name,
        decimal price,
        string? description,
        string? notes,
        TimeSpan? customerDuration,
        TimeSpan? calendarDuration,
        List<IndexedImage> images,
        List<string> termsAndConditions)
    : base(
        id,
        createdAt,
        createdBy,
        updatedAt,
        updatedBy,
        deletedAt,
        deletedBy,
        tags,
        extensions)
    {
        TenantId = tenantId;
        Name = name;
        Price = price;
        Description = description;
        Notes = notes;
        CustomerDuration = customerDuration;
        CalendarDuration = calendarDuration;
        Images = images;
        TermsAndConditions = termsAndConditions;
    }

    public static Service CreateMinimal(
        string? createdBy,
        Guid tenantId)
    {
        var service = new Service(
            Guid.NewGuid(),
            DateTime.UtcNow,
            createdBy,
            null,
            null,
            null,
            null,
            new List<string>(),
            new Dictionary<string, string?>(),

            tenantId,
            string.Empty,
            0,
            null,
            null,
            null,
            null,
            new List<IndexedImage>(),
            new List<string>());

        service.AddEvent(new MinimalServiceCreatedEvent(
            service.Id,
            createdBy,
            tenantId));

        return service;
    }
}
