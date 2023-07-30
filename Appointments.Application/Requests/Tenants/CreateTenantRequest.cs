using Appointments.Application.Repositories.Tenants;
using Appointments.Application.Services.Events;
using Appointments.Domain.Entities;
using MediatR;

namespace Appointments.Application.Requests.Tenants;

public sealed record CreateTenantRequest(
    string? CreatedBy,
    string Name,
    string? Slogan,
    string UrlId,
    List<SocialMediaContact> SocialMediaContacts,
    WeeklySchedule? WeeklySchedule
    ) : IRequest<Guid>;

internal sealed class CreateTenantRequestHandler : IRequestHandler<CreateTenantRequest, Guid>
{
    private readonly IEventProcessor _eventProcessor;
    private readonly ITenantRepository _tenantRepository;

    public CreateTenantRequestHandler(IEventProcessor eventProcessor, ITenantRepository tenantRepository)
    {
        _eventProcessor = eventProcessor;
        _tenantRepository = tenantRepository;
    }

    public async Task<Guid> Handle(CreateTenantRequest request, CancellationToken cancellationToken)
    {
        var tenant = Tenant.Create(
            request.CreatedBy,
            request.Name,
            request.Slogan,
            request.UrlId,
            null,
            request.SocialMediaContacts,
            request.WeeklySchedule);

        if (tenant.HasChanged)
        {
            await _tenantRepository.CreateAsync(tenant);
            await _eventProcessor.ProcessAsync(tenant.Events);
        }

        return tenant.Id;
    }
}
