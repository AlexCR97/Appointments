using Appointments.Application.Repositories.Services;
using Appointments.Common.MongoClient.Abstractions;
using Appointments.Domain.Entities;
using Appointments.Infrastructure.Mapper.Abstractions;
using Appointments.Infrastructure.Mongo.Documents;

namespace Appointments.Infrastructure.Repositories;

internal class ServiceRepository : EntityRepository<Service, ServiceDocument>, IServiceRepository
{
    public ServiceRepository(IMapper mapper, IMongoRepository<ServiceDocument> repository)
        : base(mapper, repository)
    {
    }
}
