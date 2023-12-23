﻿using Appointments.Application.Mapper.Abstractions;
using Appointments.Application.Requests.Tenants;
using Appointments.Common.MongoClient.Abstractions;
using Appointments.Domain.Entities;
using Appointments.Infrastructure.Mongo.Documents;

namespace Appointments.Infrastructure.Repositories;

internal class TenantRepository : EntityRepository<Tenant, TenantDocument>, ITenantRepository
{
    public TenantRepository(IMapper mapper, IMongoRepository<TenantDocument> repository)
        : base(mapper, repository)
    {
    }
}
