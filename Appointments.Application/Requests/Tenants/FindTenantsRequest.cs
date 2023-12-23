using Appointments.Domain.Entities;
using Appointments.Domain.Models;
using FluentValidation;
using MediatR;

namespace Appointments.Application.Requests.Tenants;

public sealed record FindTenantsRequest : FindRequest<Tenant>
{
    public FindTenantsRequest(int PageIndex, int PageSize, string? Sort, string? Filter) : base(PageIndex, PageSize, Sort, Filter)
    {
    }
}

internal sealed class FindTenantsRequestHandler : IRequestHandler<FindTenantsRequest, PagedResult<Tenant>>
{
    private readonly ITenantRepository _tenantRepository;

    public FindTenantsRequestHandler(ITenantRepository tenantRepository)
    {
        _tenantRepository = tenantRepository;
    }

    public async Task<PagedResult<Tenant>> Handle(FindTenantsRequest request, CancellationToken cancellationToken)
    {
        new FindRequestValidator<Tenant>().ValidateAndThrow(request);

        return await _tenantRepository.FindAsync(
            request.PageIndex,
            request.PageSize,
            request.Sort,
            request.Filter);
    }
}
