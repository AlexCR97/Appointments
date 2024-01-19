using Appointments.Core.Domain.Entities;

namespace Appointments.Api.Tenant.Models;

public sealed record BranchOfficeListResponse(
    Guid Id,
    DateTime CreatedAt,
    string CreatedBy,
    DateTime? UpdatedAt,
    string? UpdatedBy,
    Guid TenantId,
    string Name,
    AddressModel Address)
{
    internal static BranchOfficeListResponse From(BranchOffice entity)
    {
        return new BranchOfficeListResponse(
            entity.Id,
            entity.CreatedAt,
            entity.CreatedBy,
            entity.UpdatedAt,
            entity.UpdatedBy,
            entity.TenantId,
            entity.Name,
            AddressModel.From(entity.Address));
    }
}
