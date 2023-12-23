using Appointments.Application.Services.Events;
using Appointments.Application.Validations;
using Appointments.Domain.Entities;
using Appointments.Domain.Models;
using FluentValidation;
using MediatR;

namespace Appointments.Application.Requests.Customers;

public sealed record CreateCustomerRequest(
    string CreatedBy,
    string FirstName,
    string LastName,
    Email? Email,
    string? PhoneNumber)
    : IRequest<Guid>;

internal sealed class CreateCustomerRequestValidator : AbstractValidator<CreateCustomerRequest>
{
    public CreateCustomerRequestValidator()
    {
        RuleFor(x => x.CreatedBy)
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

internal sealed class CreateCustomerRequestHandler : IRequestHandler<CreateCustomerRequest, Guid>
{
    private readonly IEventProcessor _eventProcessor;
    private readonly ICustomerRepository _customerRepository;

    public CreateCustomerRequestHandler(IEventProcessor eventProcessor, ICustomerRepository customerRepository)
    {
        _eventProcessor = eventProcessor;
        _customerRepository = customerRepository;
    }

    public async Task<Guid> Handle(CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        new CreateCustomerRequestValidator().ValidateAndThrow(request);

        var customer = Customer.Create(
            request.CreatedBy,
            request.FirstName,
            request.LastName,
            request.Email,
            request.PhoneNumber);

        if (customer.HasChanged)
        {
            await _customerRepository.CreateAsync(customer);
            await _eventProcessor.ProcessAsync(customer.Events);
        }

        return customer.Id;
    }
}
