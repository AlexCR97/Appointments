using Appointments.Application.Repositories.Tenants;
using Appointments.Application.Services.Events;
using Appointments.Application.Validations.Tenants;
using Appointments.Domain.Entities;
using FluentValidation;
using MediatR;

namespace Appointments.Application.Requests.Tenants;

public sealed record UpdateTenantRequest(
    Guid Id,
    string? UpdatedBy,
    string Name,
    string? Slogan,
    string UrlId,
    List<SocialMediaContact>? SocialMediaContacts) : IRequest;

internal sealed class UpdateTenantRequestHandler : IRequestHandler<UpdateTenantRequest>
{
    private readonly IEventProcessor _eventProcessor;
    private readonly ITenantRepository _tenantRepository;

    public UpdateTenantRequestHandler(IEventProcessor eventProcessor, ITenantRepository tenantRepository)
    {
        _eventProcessor = eventProcessor;
        _tenantRepository = tenantRepository;
    }

    public async Task Handle(UpdateTenantRequest request, CancellationToken cancellationToken)
    {
        new UpdateTenantRequestValidator().ValidateAndThrow(request);

        var tenant = await _tenantRepository.GetByIdAsync(request.Id);

        tenant.Update(
            request.UpdatedBy,
            request.Name,
            request.Slogan,
            request.UrlId,
            request.SocialMediaContacts);

        if (tenant.HasChanged)
        {
            await _tenantRepository.UpdateAsync(tenant);
            await _eventProcessor.ProcessAsync(tenant.Events);
        }
    }
}
