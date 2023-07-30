using Appointments.Application.Repositories.Tenants;
using Appointments.Application.Services.Events;
using Appointments.Domain.Entities;
using MediatR;

namespace Appointments.Application.Requests.Tenants;

public sealed record UpdateScheduleRequest(
    string? UpdatedBy,
    Guid Id,
    WeeklySchedule WeeklySchedule) : IRequest;

internal sealed class UpdateScheduleRequestHandler : IRequestHandler<UpdateScheduleRequest>
{
    private readonly IEventProcessor _eventProcessor;
    private readonly ITenantRepository _tenantRepository;

    public UpdateScheduleRequestHandler(IEventProcessor eventProcessor, ITenantRepository tenantRepository)
    {
        _eventProcessor = eventProcessor;
        _tenantRepository = tenantRepository;
    }

    public async Task Handle(UpdateScheduleRequest request, CancellationToken cancellationToken)
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
