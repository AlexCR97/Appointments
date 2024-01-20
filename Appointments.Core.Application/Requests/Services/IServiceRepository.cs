using Appointments.Common.Application;
using Appointments.Core.Domain.Entities;

namespace Appointments.Core.Application.Requests.Services;

public interface IServiceRepository : IRepository<Service>
{
}
