using Appointments.Domain.Entities;
using Appointments.Domain.Models;
using FluentValidation;
using MediatR;

namespace Appointments.Application.Requests.Customers;

public sealed record FindCustomersRequest : FindRequest<Customer>
{
    public FindCustomersRequest(int PageIndex, int PageSize, string? Sort, string? Filter) : base(PageIndex, PageSize, Sort, Filter)
    {
    }
}

internal sealed class FindCustomersRequestHandler : IRequestHandler<FindCustomersRequest, PagedResult<Customer>>
{
    private readonly ICustomerRepository _customerRepository;

    public FindCustomersRequestHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<PagedResult<Customer>> Handle(FindCustomersRequest request, CancellationToken cancellationToken)
    {
        new FindRequestValidator<Customer>().ValidateAndThrow(request);

        return await _customerRepository.FindAsync(
            request.PageIndex,
            request.PageSize,
            request.Sort,
            request.Filter);
    }
}
