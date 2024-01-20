using Appointments.Common.Application;
using Appointments.Common.Domain;
using FluentValidation;
using MediatR;

namespace Appointments.Core.Application.Requests.Services;

public sealed record DeleteServiceRequest : DeleteRequest
{
    public DeleteServiceRequest(string DeletedBy, Guid Id) : base(DeletedBy, Id)
    {
    }
}

internal sealed class DeleteServiceRequestHandler : IRequestHandler<DeleteServiceRequest>
{
    private readonly IEventProcessor _eventProcessor;
    private readonly IServiceRepository _serviceRepository;

    public DeleteServiceRequestHandler(IEventProcessor eventProcessor, IServiceRepository serviceRepository)
    {
        _eventProcessor = eventProcessor;
        _serviceRepository = serviceRepository;
    }

    public async Task Handle(DeleteServiceRequest request, CancellationToken cancellationToken)
    {
        new DeleteRequestValidator().ValidateAndThrow(request);

        var service = await _serviceRepository.GetAsync(request.Id);

        await _serviceRepository.DeleteAsync(service.Id);
        await _eventProcessor.ProcessAsync(service.Events);
    }
}
