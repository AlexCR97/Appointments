using Appointments.Core.Domain.Entities;

namespace Appointments.Api.Tenant.Models;

public sealed record CreateBranchOfficeRequest(
    string Name,
    AddressModel Address,
    SocialMediaContactModel[]? Contacts,
    WeeklyScheduleModel? Schedule)
{
    internal Appointments.Core.Application.Requests.BranchOffices.CreateBranchOfficeRequest ToApplicationRequest(
        string createdBy,
        Guid tenantId)
    {
        return new Appointments.Core.Application.Requests.BranchOffices.CreateBranchOfficeRequest(
            createdBy,
            tenantId,
            Name,
            Address.ToModel(),
            Contacts
                ?.Select(x => x.ToModel())
                .ToArray()
                ?? Array.Empty<SocialMediaContact>(),
            Schedule?.ToModel());
    }
}
