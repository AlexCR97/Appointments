namespace Appointments.Api.Tenant.Models;

public sealed record CreateAppointmentRequest(
    Guid BranchOfficeId,
    Guid ServiceId,
    Guid? ServiceProviderId,
    DateTime AppointmentDate,
    AppointmentCustomerModel Customer,
    string? Notes)
{
    internal Appointments.Core.Application.Requests.Appointments.CreateAppointmentRequest ToApplicationRequest(
        string createdBy,
        Guid tenantId)
    {
        return new Appointments.Core.Application.Requests.Appointments.CreateAppointmentRequest(
            createdBy,
            tenantId,
            BranchOfficeId,
            ServiceId,
            ServiceProviderId,
            AppointmentDate,
            Customer.ToEntity(),
            Notes);
    }
}
