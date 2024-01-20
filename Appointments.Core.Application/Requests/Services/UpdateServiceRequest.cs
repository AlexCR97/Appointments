using Appointments.Common.Domain;
using Appointments.Common.Domain.Models;
using FluentValidation;
using MediatR;

namespace Appointments.Core.Application.Requests.Services;

public sealed record UpdateServiceProfileRequest(
    string UpdatedBy,
    Guid Id,
    Guid TenantId,
    string Name,
    string? Description,
    decimal Price,
    IndexedResource[] Images,
    string[] TermsAndConditions,
    string? Notes,
    TimeSpan? CustomerDuration,
    TimeSpan? CalendarDuration)
    : IRequest;

internal sealed class UpdateServiceProfileRequestValidator : AbstractValidator<UpdateServiceProfileRequest>
{
    public UpdateServiceProfileRequestValidator()
    {
        RuleFor(x => x.UpdatedBy)
            .NotEmpty();

        RuleFor(x => x.Id)
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

internal sealed class UpdateServiceProfileRequestHandler : IRequestHandler<UpdateServiceProfileRequest>
{
    private readonly IEventProcessor _eventProcessor;
    private readonly IServiceRepository _serviceRepository;

    public UpdateServiceProfileRequestHandler(IEventProcessor eventProcessor, IServiceRepository serviceRepository)
    {
        _eventProcessor = eventProcessor;
        _serviceRepository = serviceRepository;
    }

    public async Task Handle(UpdateServiceProfileRequest request, CancellationToken cancellationToken)
    {
        new UpdateServiceProfileRequestValidator().ValidateAndThrow(request);

        var service = await _serviceRepository.GetAsync(request.Id);

        service.Update(
            request.UpdatedBy,
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
            await _serviceRepository.UpdateAsync(service);
            await _eventProcessor.ProcessAsync(service.Events);
        }
    }
}
