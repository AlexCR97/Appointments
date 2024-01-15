using Appointments.Common.Domain.Models;

namespace Appointments.Api.Tenant.Models;

public sealed record CreateServiceRequest(
    string Name,
    string? Description,
    decimal Price,
    IndexedResourceModel[]? Images,
    string[]? TermsAndConditions,
    string? Notes,
    TimeSpan? CustomerDuration,
    TimeSpan? CalendarDuration)
{
    internal Appointments.Core.Application.Requests.Services.CreateServiceRequest ToApplicationRequest(
        string createdBy,
        Guid tenantId)
    {
        return new Appointments.Core.Application.Requests.Services.CreateServiceRequest(
            createdBy,
            tenantId,
            Name,
            Description,
            Price,
            Images
                ?.Select(image => image.ToModel())
                .ToArray()
                ?? Array.Empty<IndexedResource>(),
            TermsAndConditions ?? Array.Empty<string>(),
            Notes,
            CustomerDuration,
            CalendarDuration);
    }
}
