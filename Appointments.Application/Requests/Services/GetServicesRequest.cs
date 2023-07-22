using Appointments.Application.Mapper.Abstractions;
using Appointments.Application.Repositories.Services;
using Appointments.Domain.Entities;
using Appointments.Domain.Models.Services;

namespace Appointments.Application.Requests.Services;

public sealed record GetServicesRequest : GetPagedRequest<ServiceModel>
{
    public GetServicesRequest(int PageIndex, int PageSize, string? Sort, string? Filter)
        : base(PageIndex, PageSize, Sort, Filter)
    {
    }
}

internal class GetServicesRequestHandler : GetPagedRequestHandler<GetServicesRequest, Service, ServiceModel>
{
    public GetServicesRequestHandler(IMapper mapper, IServiceRepository serviceRepository) : base(mapper, serviceRepository)
    {
    }
}
