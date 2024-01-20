using Appointments.Common.Application;
using Appointments.Common.Domain;
using FluentValidation;
using MediatR;

namespace Appointments.Core.Application.Requests.Tenants;

public sealed record DeleteTenantRequest : DeleteRequest
{
    public DeleteTenantRequest(string DeletedBy, Guid Id) : base(DeletedBy, Id)
    {
    }
}

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
        new DeleteRequestValidator().ValidateAndThrow(request);

        var tenant = await _tenantRepository.GetAsync(request.Id);

        await _tenantRepository.DeleteAsync(tenant.Id);
        await _eventProcessor.ProcessAsync(tenant.Events);
    }
}
