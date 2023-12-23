using Appointments.Application.Services.Events;
using FluentValidation;
using MediatR;

namespace Appointments.Application.Requests.BranchOffices;

public sealed record DeleteBranchOfficeRequest : DeleteRequest
{
    public DeleteBranchOfficeRequest(string DeletedBy, Guid Id) : base(DeletedBy, Id)
    {
    }
}

internal sealed class DeleteBranchOfficeRequestHandler : IRequestHandler<DeleteBranchOfficeRequest>
{
    private readonly IEventProcessor _eventProcessor;
    private readonly IBranchOfficeRepository _branchOfficeRepository;

    public DeleteBranchOfficeRequestHandler(IEventProcessor eventProcessor, IBranchOfficeRepository branchOfficeRepository)
    {
        _eventProcessor = eventProcessor;
        _branchOfficeRepository = branchOfficeRepository;
    }

    public async Task Handle(DeleteBranchOfficeRequest request, CancellationToken cancellationToken)
    {
        new DeleteRequestValidator().ValidateAndThrow(request);

        var branchOffice = await _branchOfficeRepository.GetAsync(request.Id);
        
        await _branchOfficeRepository.DeleteAsync(branchOffice.Id);
        await _eventProcessor.ProcessAsync(branchOffice.Events);
    }
}
