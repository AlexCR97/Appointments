using Appointments.Core.Domain.Entities;

namespace Appointments.Api.Tenant.Models;

public sealed record UpdateBranchOfficeRequest(
    string Name,
    AddressModel Address,
    SocialMediaContactModel[]? Contacts,
    WeeklyScheduleModel? Schedule)
{
    internal Appointments.Core.Application.Requests.BranchOffices.UpdateBranchOfficeRequest ToApplicationRequest(
        Guid branchOfficeId,
        string updatedBy)
    {
        return new Appointments.Core.Application.Requests.BranchOffices.UpdateBranchOfficeRequest(
            branchOfficeId,
            updatedBy,
            Name,
            Address.ToModel(),
            Contacts
                ?.Select(x => x.ToModel())
                .ToArray()
                ?? Array.Empty<SocialMediaContact>());
    }
}
