using Appointments.Domain.Entities;
using FluentValidation;
using MediatR;

namespace Appointments.Application.Requests.Customers;

public sealed record GetCustomerRequest : GetRequest<Customer>
{
    public GetCustomerRequest(Guid Id) : base(Id)
    {
    }
}

internal sealed class GetCustomerRequestHandler : IRequestHandler<GetCustomerRequest, Customer>
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomerRequestHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<Customer> Handle(GetCustomerRequest request, CancellationToken cancellationToken)
    {
        new GetRequestValidator<Customer>().ValidateAndThrow(request);

        return await _customerRepository.GetAsync(request.Id);
    }
}
