using Appointments.Common.Application;
using Appointments.Core.Domain.Entities;

namespace Appointments.Core.Application.Requests.Tenants;

public interface ITenantRepository : IRepository<Tenant>
{
}
