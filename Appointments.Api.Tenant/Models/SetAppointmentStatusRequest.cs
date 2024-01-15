using Appointments.Common.Domain.Enums;
using Appointments.Core.Domain.Entities;

namespace Appointments.Api.Tenant.Models;

public sealed record SetAppointmentStatusRequest(
    string Status)
{
    internal Appointments.Core.Application.Requests.Appointments.SetAppointmentStatusRequest ToApplicationRequest(
        string updatedBy,
        Guid appointmentId)
    {
        return new Appointments.Core.Application.Requests.Appointments.SetAppointmentStatusRequest(
            updatedBy,
            appointmentId,
            Status.ToEnum<AppointmentStatus>());
    }
}
