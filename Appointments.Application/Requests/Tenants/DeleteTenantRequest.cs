using Appointments.Application.Repositories.Tenants;
using Appointments.Application.Services.Events;
using MediatR;

namespace Appointments.Application.Requests.Tenants;

public sealed record DeleteTenantRequest(
    string? DeletedBy,
    Guid TenantId) : IRequest;

internal sealed class DeleteTenantRequestHandler : IRequestHandler<DeleteTenantRequest>
{
    private readonly IEventProcessor _eventProcessor;
    private readonly ITenantRepository _tenantRepository;

    public DeleteTenantRequestHandler(IEventProcessor eventProcessor, ITenantRepository tenantRepository)
    {
        _eventProcessor = eventProcessor;
        _tenantRepository = tenantRepository;
    }

    public async Task Handle(DeleteTenantRequest request, CancellationToken cancellationToken)
    {
        var tenant = await _tenantRepository.GetByIdAsync(request.TenantId);

        tenant.Delete(request.DeletedBy);

        if (tenant.HasChanged)
        {
            await _tenantRepository.DeleteAsync(tenant.Id);
            await _eventProcessor.ProcessAsync(tenant.Events);
        }
    }
}
