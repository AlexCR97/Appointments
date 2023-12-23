using Appointments.Application.Services.Events;
using Appointments.Domain.Entities;
using FluentValidation;
using MediatR;

namespace Appointments.Application.Requests.Tenants;

public sealed record UpdateTenantScheduleRequest(
    Guid Id,
    string UpdatedBy,
    WeeklySchedule? WeeklySchedule)
    : IRequest;

internal sealed class UpdateTenantScheduleRequestValidator : AbstractValidator<UpdateTenantScheduleRequest>
{
    public UpdateTenantScheduleRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.UpdatedBy)
            .NotEmpty();

        When(x => x.WeeklySchedule is not null, () =>
        {
            RuleFor(x => x.WeeklySchedule)
                .SetValidator(new WeeklyScheduleValidator());
        });
    }
}

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
        var tenant = await _tenantRepository.GetAsync(request.Id);

        tenant.SetSchedule(
            request.UpdatedBy,
            request.WeeklySchedule);

        if (tenant.HasChanged)
        {
            await _tenantRepository.UpdateAsync(tenant);
            await _eventProcessor.ProcessAsync(tenant.Events);
        }
    }
}
