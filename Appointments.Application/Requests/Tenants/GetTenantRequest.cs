using Appointments.Application.Mapper.Abstractions;
using Appointments.Application.Repositories.Tenants;
using Appointments.Domain.Models.Tenants;
using MediatR;

namespace Appointments.Application.Requests.Tenants;

public record GetTenantRequest(
    Guid Id) : IRequest<TenantModel>;

internal class GetTenantRequestHandler : IRequestHandler<GetTenantRequest, TenantModel>
{
    private readonly IMapper _mapper;
    private readonly ITenantRepository _tenantRepository;

    public GetTenantRequestHandler(IMapper mapper, ITenantRepository tenantRepository)
    {
        _mapper = mapper;
        _tenantRepository = tenantRepository;
    }

    public async Task<TenantModel> Handle(GetTenantRequest request, CancellationToken cancellationToken)
    {
        var tenant = await _tenantRepository.GetByIdAsync(request.Id);
        return _mapper.Map<TenantModel>(tenant);
    }
}
