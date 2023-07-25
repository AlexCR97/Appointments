using Appointments.Application.Repositories.BranchOffices;
using Appointments.Application.Services.Events;
using Appointments.Domain.Entities;
using MediatR;

namespace Appointments.Application.Requests.BranchOffices;

public sealed record UpdateBranchOfficeRequest(
    string? UpdatedBy,
    Guid Id,
    string Name,
    Location Location,
    string Address,
    List<SocialMediaContact> SocialMediaContacts) : IRequest;

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
        var branchOffice = await _branchOfficeRepository.GetByIdAsync(request.Id);

        branchOffice.Update(
            request.UpdatedBy,
            request.Name,
            request.Location,
            request.Address,
            request.SocialMediaContacts);

        if (branchOffice.HasChanged)
        {
            await _branchOfficeRepository.UpdateAsync(branchOffice);
            await _eventProcessor.ProcessAsync(branchOffice.Events);
        }
    }
}
