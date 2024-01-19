using Appointments.Core.Domain.Entities;

namespace Appointments.Api.Tenant.Models;

public sealed record AppointmentProfileResponse(
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
    AppointmentCustomerModel Customer,
    string? Notes,
    string Status)
{
    internal static AppointmentProfileResponse From(Appointment appointment)
    {
        return new AppointmentProfileResponse(
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
            AppointmentCustomerModel.From(appointment.Customer),
            appointment.Notes,
            appointment.Status.ToString());
    }
}
