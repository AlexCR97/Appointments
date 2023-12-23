using Appointments.Application.Services.Events;
using Appointments.Domain.Entities;
using Appointments.Domain.Models;
using MediatR;

namespace Appointments.Application.Requests.BranchOffices;

public sealed record UpdateBranchOfficeRequest(
    Guid Id,
    string UpdatedBy,
    string Name,
    Address Address,
    SocialMediaContact[] Contacts)
    : IRequest;

internal class UpdateBranchOfficeRequestHandler : IRequestHandler<UpdateBranchOfficeRequest>
{
    private readonly IBranchOfficeRepository _branchOfficeRepository;
    private readonly IEventProcessor _eventProcessor;

    public UpdateBranchOfficeRequestHandler(IBranchOfficeRepository branchOfficeRepository, IEventProcessor eventProcessor)
    {
        _branchOfficeRepository = branchOfficeRepository;
        _eventProcessor = eventProcessor;
    }

    public async Task Handle(UpdateBranchOfficeRequest request, CancellationToken cancellationToken)
    {
        var branchOffice = await _branchOfficeRepository.GetAsync(request.Id);

        branchOffice.Update(
            request.UpdatedBy,
            request.Name,
            request.Address,
            request.Contacts);

        if (branchOffice.HasChanged)
        {
            await _branchOfficeRepository.UpdateAsync(branchOffice);
            await _eventProcessor.ProcessAsync(branchOffice.Events);
        }
    }
}
