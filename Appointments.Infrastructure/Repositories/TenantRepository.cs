using Appointments.Application.Repositories.Tenants;
using Appointments.Common.MongoClient.Abstractions;
using Appointments.Domain.Entities;
using Appointments.Infrastructure.Mapper.Abstractions;
using Appointments.Infrastructure.Mongo.Documents;

namespace Appointments.Infrastructure.Repositories;

internal class TenantRepository : EntityRepository<Tenant, TenantDocument>, ITenantRepository
{
    public TenantRepository(IMapper mapper, IMongoRepository<TenantDocument> repository)
        : base(mapper, repository)
    {
    }
}
