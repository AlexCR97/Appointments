using Appointments.Application.Services.Events;
using Appointments.Application.Validations;
using Appointments.Application.Validations.Tenants;
using Appointments.Domain.Entities;
using FluentValidation;
using MediatR;

namespace Appointments.Application.Requests.Tenants;

public sealed record UpdateTenantProfileRequest(
    string UpdatedBy,
    Guid Id,
    string Name,
    string? Slogan,
    TenantUrlId UrlId,
    SocialMediaContact[] Contacts)
    : IRequest;

internal sealed class UpdateTenantProfileRequestValidator : AbstractValidator<UpdateTenantProfileRequest>
{
    public UpdateTenantProfileRequestValidator()
    {
        RuleFor(x => x.UpdatedBy)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty();

        RuleFor(x => x.UrlId)
            .SetValidator(new TenantUrlIdValidator());

        RuleForEach(x => x.Contacts)
            .SetValidator(new SocialMediaContactValidator());
    }
}

internal sealed class UpdateTenantRequestHandler : IRequestHandler<UpdateTenantProfileRequest>
{
    private readonly IEventProcessor _eventProcessor;
    private readonly ITenantRepository _tenantRepository;

    public UpdateTenantRequestHandler(IEventProcessor eventProcessor, ITenantRepository tenantRepository)
    {
        _eventProcessor = eventProcessor;
        _tenantRepository = tenantRepository;
    }

    public async Task Handle(UpdateTenantProfileRequest request, CancellationToken cancellationToken)
    {
        new UpdateTenantRequestValidator().ValidateAndThrow(request);

        var tenant = await _tenantRepository.GetAsync(request.Id);

        tenant.UpdateProfile(
            request.UpdatedBy,
            request.Name,
            request.Slogan,
            request.UrlId,
            request.Contacts);

        if (tenant.HasChanged)
        {
            await _tenantRepository.UpdateAsync(tenant);
            await _eventProcessor.ProcessAsync(tenant.Events);
        }
    }
}
