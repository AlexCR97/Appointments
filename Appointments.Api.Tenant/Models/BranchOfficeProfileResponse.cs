using Appointments.Core.Domain.Entities;

namespace Appointments.Api.Tenant.Models;

public sealed record BranchOfficeProfileResponse(
    Guid Id,
    DateTime CreatedAt,
    string CreatedBy,
    DateTime? UpdatedAt,
    string? UpdatedBy,
    Guid TenantId,
    string Name,
    AddressModel Address,
    SocialMediaContactModel[] Contacts,
    WeeklyScheduleModel? Schedule)
{
    internal static BranchOfficeProfileResponse From(BranchOffice entity)
    {
        return new BranchOfficeProfileResponse(
            entity.Id,
            entity.CreatedAt,
            entity.CreatedBy,
            entity.UpdatedAt,
            entity.UpdatedBy,
            entity.TenantId,
            entity.Name,
            AddressModel.From(entity.Address),
            entity.Contacts
                .Select(SocialMediaContactModel.From)
                .ToArray(),
            WeeklyScheduleModel.FromOrDefault(entity.Schedule));
    }
}
