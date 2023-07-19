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

    private Service(
        string? createdBy,
        Guid tenantId,
        string name,
        decimal price,
        string? description,
        string? notes,
        TimeSpan? customerDuration,
        TimeSpan? calendarDuration,
        List<IndexedImage> images,
        List<string> termsAndConditions)
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        CreatedBy = createdBy;

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

    public static Service CreateSlim(
        string? createdBy,
        Guid tenantId)
    {
        var service = new Service(
            createdBy,
            tenantId,
            string.Empty,
            0,
            null,
            null,
            null,
            null,
            new List<IndexedImage>(),
            new List<string>());

        service.AddEvent(new SlimServiceCreatedEvent(
            service.Id,
            createdBy,
            tenantId));

        return service;
    }
}
