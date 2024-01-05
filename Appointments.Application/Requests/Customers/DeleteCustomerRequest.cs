using Appointments.Common.Application;
using Appointments.Common.Domain;
using FluentValidation;
using MediatR;

namespace Appointments.Core.Application.Requests.Customers;

public sealed record DeleteCustomerRequest : DeleteRequest
{
    public DeleteCustomerRequest(string DeletedBy, Guid Id) : base(DeletedBy, Id)
    {
    }
}

internal sealed class DeleteCustomerRequestHandler : IRequestHandler<DeleteCustomerRequest>
{
    private readonly IEventProcessor _eventProcessor;
    private readonly ICustomerRepository _customerRepository;

    public DeleteCustomerRequestHandler(IEventProcessor eventProcessor, ICustomerRepository customerRepository)
    {
        _eventProcessor = eventProcessor;
        _customerRepository = customerRepository;
    }

    public async Task Handle(DeleteCustomerRequest request, CancellationToken cancellationToken)
    {
        new DeleteRequestValidator().ValidateAndThrow(request);

        var customer = await _customerRepository.GetAsync(request.Id);

        await _customerRepository.DeleteAsync(customer.Id);
        await _eventProcessor.ProcessAsync(customer.Events);
    }
}
