using Appointments.Core.Domain.Entities;

namespace Appointments.Api.Tenant.Controllers;

public sealed record AppointmentListResponse(
    Guid Id,
    DateTime CreatedAt,
    string CreatedBy,
    DateTime? UpdatedAt,
    string? UpdatedBy,
    Guid TenantId,
    Guid BranchOfficeId,
    Guid ServiceId,
    Guid? ServiceProviderId,
    DateTime AppointmentDate,
    string AppointmentCode,
    string Status)
{
    internal static AppointmentListResponse From(Appointment appointment)
    {
        return new AppointmentListResponse(
            appointment.Id,
            appointment.CreatedAt,
            appointment.CreatedBy,
            appointment.UpdatedAt,
            appointment.UpdatedBy,
            appointment.TenantId,
            appointment.BranchOfficeId,
            appointment.ServiceId,
            appointment.ServiceProviderId,
            appointment.AppointmentDate,
            appointment.AppointmentCode.Value,
            appointment.Status.ToString());
    }
}
