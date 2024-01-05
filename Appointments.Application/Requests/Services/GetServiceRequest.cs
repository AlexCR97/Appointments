using Appointments.Common.Application;
using Appointments.Core.Domain.Entities;
using FluentValidation;
using MediatR;

namespace Appointments.Core.Application.Requests.Services;

public sealed record GetServiceRequest : GetRequest<Service>
{
    public GetServiceRequest(Guid Id) : base(Id)
    {
    }
}

internal sealed class GetServiceRequestHandler : IRequestHandler<GetServiceRequest, Service>
{
    private readonly IServiceRepository _serviceRepository;

    public GetServiceRequestHandler(IServiceRepository serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }

    public async Task<Service> Handle(GetServiceRequest request, CancellationToken cancellationToken)
    {
        new GetRequestValidator<Service>().ValidateAndThrow(request);

        return await _serviceRepository.GetAsync(request.Id);
    }
}
