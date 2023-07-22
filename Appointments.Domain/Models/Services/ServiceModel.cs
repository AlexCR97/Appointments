using Appointments.Domain.Entities;

namespace Appointments.Domain.Models.Services;

public class ServiceModel
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }

    public Guid TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }

    /// <summary>
    /// The amount of time it takes for the customer to receive the service.
    /// </summary>
    public TimeSpan? CustomerDuration { get; set; }

    /// <summary>
    /// The amount of time the service occupies in the calendar.
    /// </summary>
    public TimeSpan? CalendarDuration { get; set; }

    public List<IndexedImage> Images { get; set; } = new();
    public List<string> TermsAndConditions { get; set; } = new();
}
