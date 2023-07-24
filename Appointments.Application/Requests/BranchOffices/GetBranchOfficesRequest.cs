using Appointments.Application.Mapper.Abstractions;
using Appointments.Application.Repositories.BranchOffices;
using Appointments.Domain.Entities;
using Appointments.Domain.Models.BranchOffices;

namespace Appointments.Application.Requests.BranchOffices;

public sealed record GetBranchOfficesRequest : GetPagedRequest<BranchOfficeModel>
{
    public GetBranchOfficesRequest(int PageIndex, int PageSize, string? Sort, string? Filter)
        : base(PageIndex, PageSize, Sort, Filter)
    {
    }
}

internal sealed class GetBranchOfficesRequestHandler : GetPagedRequestHandler<GetBranchOfficesRequest, BranchOffice, BranchOfficeModel>
{
    public GetBranchOfficesRequestHandler(IMapper mapper, IBranchOfficeRepository serviceRepository)
        : base(mapper, serviceRepository)
    {
    }
}
