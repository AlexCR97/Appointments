using Appointments.Common.Application;
using Appointments.Common.Domain.Models;
using Appointments.Core.Domain.Entities;
using FluentValidation;
using MediatR;

namespace Appointments.Core.Application.Requests.Services;

public sealed record FindServicesRequest : FindRequest<Service>
{
    public FindServicesRequest(int PageIndex, int PageSize, string? Sort, string? Filter) : base(PageIndex, PageSize, Sort, Filter)
    {
    }
}

internal sealed class FindServicesRequestHandler : IRequestHandler<FindServicesRequest, PagedResult<Service>>
{
    private readonly IServiceRepository _serviceRepository;

    public FindServicesRequestHandler(IServiceRepository serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }

    public async Task<PagedResult<Service>> Handle(FindServicesRequest request, CancellationToken cancellationToken)
    {
        new FindRequestValidator<Service>().ValidateAndThrow(request);

        return await _serviceRepository.FindAsync(
            request.PageIndex,
            request.PageSize,
            request.Sort,
            request.Filter);
    }
}
