﻿using Appointments.Application.Mapper.Abstractions;
using Appointments.Application.Requests.BranchOffices;
using Appointments.Common.MongoClient.Abstractions;
using Appointments.Domain.Entities;
using Appointments.Infrastructure.Mongo.Documents;

namespace Appointments.Infrastructure.Repositories;

internal class BranchOfficeRepository : EntityRepository<BranchOffice, BranchOfficeDocument>, IBranchOfficeRepository
{
    public BranchOfficeRepository(IMapper mapper, IMongoRepository<BranchOfficeDocument> repository)
        : base(mapper, repository)
    {
    }
}
