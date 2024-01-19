using Appointments.Common.Domain.Models;

namespace Appointments.Api.Tenant.Models;

public sealed record UpdateServiceRequest(
    string Name,
    string? Description,
    decimal Price,
    IndexedResourceModel[]? Images,
    string[]? TermsAndConditions,
    string? Notes,
    TimeSpan? CustomerDuration,
    TimeSpan? CalendarDuration)
{
    internal Appointments.Core.Application.Requests.Services.UpdateServiceProfileRequest ToApplicationRequest(
        string updatedBy,
        Guid serviceId,
        Guid tenantId)
    {
        return new Appointments.Core.Application.Requests.Services.UpdateServiceProfileRequest(
            updatedBy,
            serviceId,
            tenantId,
            Name,
            Description,
            Price,
            Images
                ?.Select(x => x.ToModel())
                .ToArray()
                ?? Array.Empty<IndexedResource>(),
            TermsAndConditions ?? Array.Empty<string>(),
            Notes,
            CustomerDuration,
            CalendarDuration);
    }
}
