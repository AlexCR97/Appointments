using Appointments.Application.Repositories.Tenants;
using Appointments.Application.Services.Events;
using Appointments.Domain.Entities;
using MediatR;

namespace Appointments.Application.Requests.Tenants;

public sealed record UpdateTenantScheduleRequest(
    string? UpdatedBy,
    Guid Id,
    WeeklySchedule WeeklySchedule) : IRequest;

internal sealed class UpdateTenantScheduleRequestHandler : IRequestHandler<UpdateTenantScheduleRequest>
{
    private readonly IEventProcessor _eventProcessor;
    private readonly ITenantRepository _tenantRepository;

    public UpdateTenantScheduleRequestHandler(IEventProcessor eventProcessor, ITenantRepository tenantRepository)
    {
        _eventProcessor = eventProcessor;
        _tenantRepository = tenantRepository;
    }

    public async Task Handle(UpdateTenantScheduleRequest request, CancellationToken cancellationToken)
    {
        var tenant = await _tenantRepository.GetByIdAsync(request.Id);

        tenant.UpdateSchedule(
            request.UpdatedBy,
            request.WeeklySchedule);

        if (tenant.HasChanged)
        {
            await _tenantRepository.UpdateAsync(tenant);
            await _eventProcessor.ProcessAsync(tenant.Events);
        }
    }
}
