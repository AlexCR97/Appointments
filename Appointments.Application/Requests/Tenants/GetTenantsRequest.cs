using Appointments.Application.Repositories.Tenants;
using Appointments.Application.Validations;
using Appointments.Domain.Entities;
using Appointments.Domain.Models;
using FluentValidation;
using MediatR;

namespace Appointments.Application.Requests.Tenants;

public sealed record GetTenantsRequest : GetPagedRequest<Tenant>
{
    public GetTenantsRequest(int PageIndex, int PageSize, string? Sort, string? Filter)
        : base(PageIndex, PageSize, Sort, Filter)
    {
    }
}

internal sealed class GetTenantsRequestHandler : IRequestHandler<GetTenantsRequest, PagedResult<Tenant>>
{
    private readonly ITenantRepository _tenantRepository;

    public GetTenantsRequestHandler(ITenantRepository tenantRepository)
    {
        _tenantRepository = tenantRepository;
    }

    public async Task<PagedResult<Tenant>> Handle(GetTenantsRequest request, CancellationToken cancellationToken)
    {
        new GetPagedRequestValidator<Tenant>().ValidateAndThrow(request);

        return await _tenantRepository.GetAsync(
            request.PageIndex,
            request.PageSize,
            request.Sort,
            request.Filter);
    }
}
