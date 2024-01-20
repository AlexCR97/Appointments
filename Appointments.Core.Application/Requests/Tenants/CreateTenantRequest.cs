using Appointments.Common.Domain;
using Appointments.Core.Domain.Entities;
using FluentValidation;
using MediatR;

namespace Appointments.Core.Application.Requests.Tenants;

public sealed record CreateTenantRequest(
    string CreatedBy,
    string Name,
    string? Slogan,
    TenantUrlId? UrlId,
    string? Logo,
    SocialMediaContact[] Contacts,
    WeeklySchedule? Schedule)
    : IRequest<Guid>;

internal sealed class CreateTenantRequestValidator : AbstractValidator<CreateTenantRequest>
{
    public CreateTenantRequestValidator()
    {
        RuleFor(x => x.CreatedBy)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty();

        When(x => x.UrlId is not null, () =>
        {
            RuleFor(x => x.UrlId!.Value)
                .SetValidator(new TenantUrlIdValidator());
        });

        RuleForEach(x => x.Contacts)
            .SetValidator(new SocialMediaContactValidator());

        When(x => x.Schedule is not null, () => RuleFor(x => x.Schedule!)
            .SetValidator(new WeeklyScheduleValidator()));
    }
}

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
        new CreateTenantRequestValidator().ValidateAndThrow(request);

        var tenant = Tenant.Create(
            request.CreatedBy,
            request.Name,
            request.Slogan,
            request.UrlId ?? TenantUrlId.Random(),
            request.Logo,
            request.Contacts,
            request.Schedule);

        if (tenant.HasChanged)
        {
            await _tenantRepository.CreateAsync(tenant);
            await _eventProcessor.ProcessAsync(tenant.Events);
        }

        return tenant.Id;
    }
}
