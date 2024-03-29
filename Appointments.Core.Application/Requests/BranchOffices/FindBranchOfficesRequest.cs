﻿using Appointments.Common.Application;
using Appointments.Common.Domain.Models;
using Appointments.Core.Domain.Entities;
using FluentValidation;
using MediatR;

namespace Appointments.Core.Application.Requests.BranchOffices;

public sealed record FindBranchOfficesRequest : FindRequest<BranchOffice>
{
    public FindBranchOfficesRequest(int PageIndex, int PageSize, string? Sort, string? Filter) : base(PageIndex, PageSize, Sort, Filter)
    {
    }
}

internal sealed class FindBranchOfficesRequestHandler : IRequestHandler<FindBranchOfficesRequest, PagedResult<BranchOffice>>
{
    private readonly IBranchOfficeRepository _branchOfficeRepository;

    public FindBranchOfficesRequestHandler(IBranchOfficeRepository branchOfficeRepository)
    {
        _branchOfficeRepository = branchOfficeRepository;
    }

    public async Task<PagedResult<BranchOffice>> Handle(FindBranchOfficesRequest request, CancellationToken cancellationToken)
    {
        new FindRequestValidator<BranchOffice>().ValidateAndThrow(request);

        return await _branchOfficeRepository.FindAsync(
            request.PageIndex,
            request.PageSize,
            request.Sort,
            request.Filter);
    }
}
