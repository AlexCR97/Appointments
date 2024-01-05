using Appointments.Common.Application;
using Appointments.Core.Domain.Entities;

namespace Appointments.Core.Application.Requests.Appointments;

public interface IAppointmentRepository : IRepository<Appointment>
{
}
