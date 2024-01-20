using Appointments.Common.Application;
using Appointments.Core.Domain.Entities;
using FluentValidation;
using MediatR;

namespace Appointments.Core.Application.Requests.BranchOffices;

public sealed record GetBranchOfficeRequest : GetRequest<BranchOffice>
{
    public GetBranchOfficeRequest(Guid Id) : base(Id)
    {
    }
}

internal sealed class GetBranchOfficeRequestHandler : IRequestHandler<GetBranchOfficeRequest, BranchOffice>
{
    private readonly IBranchOfficeRepository _branchOfficeRepository;

    public GetBranchOfficeRequestHandler(IBranchOfficeRepository branchOfficeRepository)
    {
        _branchOfficeRepository = branchOfficeRepository;
    }

    public async Task<BranchOffice> Handle(GetBranchOfficeRequest request, CancellationToken cancellationToken)
    {
        new GetRequestValidator<BranchOffice>().ValidateAndThrow(request);

        return await _branchOfficeRepository.GetAsync(request.Id);
    }
}
