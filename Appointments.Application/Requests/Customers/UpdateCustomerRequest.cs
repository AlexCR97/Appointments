using Appointments.Application.Services.Events;
using Appointments.Application.Validations;
using Appointments.Domain.Entities;
using FluentValidation;
using MediatR;

namespace Appointments.Application.Requests.Customers;

public sealed record UpdateCustomerProfileRequest(
    string UpdatedBy,
    Guid Id,
    string Name,
    string? Slogan,
    CustomerUrlId UrlId,
    SocialMediaContact[] Contacts)
    : IRequest;

internal sealed class UpdateCustomerProfileRequestValidator : AbstractValidator<UpdateCustomerProfileRequest>
{
    public UpdateCustomerProfileRequestValidator()
    {
        RuleFor(x => x.UpdatedBy)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty();

        RuleFor(x => x.UrlId)
            .SetValidator(new CustomerUrlIdValidator());

        RuleForEach(x => x.Contacts)
            .SetValidator(new SocialMediaContactValidator());
    }
}

internal sealed class UpdateCustomerRequestHandler : IRequestHandler<UpdateCustomerProfileRequest>
{
    private readonly IEventProcessor _eventProcessor;
    private readonly ICustomerRepository _customerRepository;

    public UpdateCustomerRequestHandler(IEventProcessor eventProcessor, ICustomerRepository customerRepository)
    {
        _eventProcessor = eventProcessor;
        _customerRepository = customerRepository;
    }

    public async Task Handle(UpdateCustomerProfileRequest request, CancellationToken cancellationToken)
    {
        new UpdateCustomerRequestValidator().ValidateAndThrow(request);

        var customer = await _customerRepository.GetAsync(request.Id);

        customer.UpdateProfile(
            request.UpdatedBy,
            request.Name,
            request.Slogan,
            request.UrlId,
            request.Contacts);

        if (customer.HasChanged)
        {
            await _customerRepository.UpdateAsync(customer);
            await _eventProcessor.ProcessAsync(customer.Events);
        }
    }
}
