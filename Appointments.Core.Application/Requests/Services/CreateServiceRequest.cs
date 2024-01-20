using Appointments.Common.Domain;
using Appointments.Common.Domain.Models;
using Appointments.Core.Domain.Entities;
using FluentValidation;
using MediatR;

namespace Appointments.Core.Application.Requests.Services;

public sealed record CreateServiceRequest(
    string CreatedBy,
    Guid TenantId,
    string Name,
    string? Description,
    decimal Price,
    IndexedResource[] Images,
    string[] TermsAndConditions,
    string? Notes,
    TimeSpan? CustomerDuration,
    TimeSpan? CalendarDuration)
    : IRequest<Guid>;

internal sealed class CreateServiceRequestValidator : AbstractValidator<CreateServiceRequest>
{
    public CreateServiceRequestValidator()
    {
        RuleFor(x => x.CreatedBy)
            .NotEmpty();

        RuleFor(x => x.TenantId)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty();

        When(x => x.Description is not null, () => RuleFor(x => x.Description)
            .NotEmpty());

        RuleForEach(x => x.Images)
            .SetValidator(new IndexedResourceValidator());

        RuleForEach(x => x.TermsAndConditions)
            .NotEmpty();

        When(x => x.Notes is not null, () => RuleFor(x => x.Notes)
            .NotEmpty());
    }
}

internal sealed class CreateServiceRequestHandler : IRequestHandler<CreateServiceRequest, Guid>
{
    private readonly IEventProcessor _eventProcessor;
    private readonly IServiceRepository _serviceRepository;

    public CreateServiceRequestHandler(IEventProcessor eventProcessor, IServiceRepository serviceRepository)
    {
        _eventProcessor = eventProcessor;
        _serviceRepository = serviceRepository;
    }

    public async Task<Guid> Handle(CreateServiceRequest request, CancellationToken cancellationToken)
    {
        new CreateServiceRequestValidator().ValidateAndThrow(request);

        var service = Service.Create(
            request.CreatedBy,
            request.TenantId,
            request.Name,
            request.Description,
            request.Price,
            request.Images.ToList(),
            request.TermsAndConditions.ToList(),
            request.Notes,
            request.CustomerDuration,
            request.CalendarDuration);

        if (service.HasChanged)
        {
            await _serviceRepository.CreateAsync(service);
            await _eventProcessor.ProcessAsync(service.Events);
        }

        return service.Id;
    }
}
