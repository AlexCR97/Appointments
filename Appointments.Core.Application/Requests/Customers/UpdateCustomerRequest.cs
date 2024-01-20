using Appointments.Common.Domain;
using Appointments.Common.Domain.Models;
using FluentValidation;
using MediatR;

namespace Appointments.Core.Application.Requests.Customers;

public sealed record UpdateCustomerProfileRequest(
    string UpdatedBy,
    Guid Id,
    string FirstName,
    string LastName,
    Email? Email,
    string? PhoneNumber)
    : IRequest;

internal sealed class UpdateCustomerProfileRequestValidator : AbstractValidator<UpdateCustomerProfileRequest>
{
    public UpdateCustomerProfileRequestValidator()
    {
        RuleFor(x => x.UpdatedBy)
            .NotEmpty();

        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.FirstName)
            .NotEmpty();

        RuleFor(x => x.LastName)
            .NotEmpty();

        When(x => x.Email is not null, () =>
        {
            RuleFor(x => x.Email!.Value)
                .SetValidator(new EmailValidator());
        });

        When(x => x.PhoneNumber is not null, () =>
        {
            RuleFor(x => x.PhoneNumber)
                .NotEmpty();
        });
    }
}

internal sealed class UpdateCustomerProfileRequestHandler : IRequestHandler<UpdateCustomerProfileRequest>
{
    private readonly IEventProcessor _eventProcessor;
    private readonly ICustomerRepository _customerRepository;

    public UpdateCustomerProfileRequestHandler(IEventProcessor eventProcessor, ICustomerRepository customerRepository)
    {
        _eventProcessor = eventProcessor;
        _customerRepository = customerRepository;
    }

    public async Task Handle(UpdateCustomerProfileRequest request, CancellationToken cancellationToken)
    {
        new UpdateCustomerProfileRequestValidator().ValidateAndThrow(request);

        var customer = await _customerRepository.GetAsync(request.Id);

        customer.Update(
            request.UpdatedBy,
            request.FirstName,
            request.LastName,
            request.Email,
            request.PhoneNumber);

        if (customer.HasChanged)
        {
            await _customerRepository.UpdateAsync(customer);
            await _eventProcessor.ProcessAsync(customer.Events);
        }
    }
}
