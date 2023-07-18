namespace Appointments.Domain.Entities;

public class Service : Entity
{
    public Guid TenantId { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public List<IndexedImage>? Images { get; set; }
    public List<string>? TermsAndConditions { get; set; }
    public string? Notes { get; set; }

    /// <summary>
    /// The amount of time it takes for the customer to receive the service.
    /// </summary>
    public TimeSpan? CustomerDuration { get; set; }

    /// <summary>
    /// The amount of time the service occupies in the calendar.
    /// </summary>
    public TimeSpan? CalendarDuration { get; set; }
}
