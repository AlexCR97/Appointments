using Appointments.Common.Application;
using Appointments.Core.Domain.Entities;
using FluentValidation;
using MediatR;

namespace Appointments.Core.Application.Requests.Tenants;

public sealed record GetTenantRequest : GetRequest<Tenant>
{
    public GetTenantRequest(Guid Id) : base(Id)
    {
    }
}

internal sealed class GetTenantRequestHandler : IRequestHandler<GetTenantRequest, Tenant>
{
    private readonly ITenantRepository _tenantRepository;

    public GetTenantRequestHandler(ITenantRepository tenantRepository)
    {
        _tenantRepository = tenantRepository;
    }

    public async Task<Tenant> Handle(GetTenantRequest request, CancellationToken cancellationToken)
    {
        new GetRequestValidator<Tenant>().ValidateAndThrow(request);

        return await _tenantRepository.GetAsync(request.Id);
    }
}
