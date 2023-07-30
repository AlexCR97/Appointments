using Appointments.Application.Mapper.Abstractions;
using Appointments.Application.Repositories.Tenants;
using Appointments.Domain.Models.Tenants;
using MediatR;

namespace Appointments.Application.Requests.Tenants;

public record GetTenantByIdRequest(
    Guid Id) : IRequest<TenantModel>;

internal class GetTenantByIdRequestHandler : IRequestHandler<GetTenantByIdRequest, TenantModel>
{
    private readonly IMapper _mapper;
    private readonly ITenantRepository _tenantRepository;

    public GetTenantByIdRequestHandler(IMapper mapper, ITenantRepository tenantRepository)
    {
        _mapper = mapper;
        _tenantRepository = tenantRepository;
    }

    public async Task<TenantModel> Handle(GetTenantByIdRequest request, CancellationToken cancellationToken)
    {
        var tenant = await _tenantRepository.GetByIdAsync(request.Id);
        return _mapper.Map<TenantModel>(tenant);
    }
}
