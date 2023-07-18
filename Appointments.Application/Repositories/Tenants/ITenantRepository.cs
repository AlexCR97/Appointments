using Appointments.Application.Repositories.Abstractions;
using Appointments.Domain.Entities;

namespace Appointments.Application.Repositories.Tenants;

public interface ITenantRepository : IRepository<Tenant>
{
}
