﻿using Appointments.Domain.Entities;

namespace Appointments.Domain.Models.Services;

public class ServiceModel : EntityModel
{
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
