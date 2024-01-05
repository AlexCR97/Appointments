using Appointments.Common.Domain;
using Appointments.Common.Domain.Models;
using Appointments.Core.Domain.Entities;
using FluentValidation;
using MediatR;

namespace Appointments.Core.Application.Requests.BranchOffices;

public sealed record CreateBranchOfficeRequest(
    string CreatedBy,
    Guid TenantId,
    string Name,
    Address Address,
    SocialMediaContact[] Contacts,
    WeeklySchedule? Schedule)
    : IRequest<Guid>;

internal sealed class CreateBranchOfficeRequestValidator : AbstractValidator<CreateBranchOfficeRequest>
{
    public CreateBranchOfficeRequestValidator()
    {
        RuleFor(x => x.CreatedBy)
            .NotEmpty();

        RuleFor(x => x.TenantId)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty();

        RuleFor(x => x.Address)
            .SetValidator(new AddressValidator());

        RuleForEach(x => x.Contacts)
            .SetValidator(new SocialMediaContactValidator());

        When(x => x.Schedule is not null, () =>
        {
            RuleFor(x => x.Schedule!)
                .SetValidator(new WeeklyScheduleValidator());
        });
    }
}

internal sealed class CreateBranchOfficeRequestHandler : IRequestHandler<CreateBranchOfficeRequest, Guid>
{
    private readonly IEventProcessor _eventProcessor;
    private readonly IBranchOfficeRepository _branchOfficeRepository;

    public CreateBranchOfficeRequestHandler(IEventProcessor eventProcessor, IBranchOfficeRepository branchOfficeRepository)
    {
        _eventProcessor = eventProcessor;
        _branchOfficeRepository = branchOfficeRepository;
    }

    public async Task<Guid> Handle(CreateBranchOfficeRequest request, CancellationToken cancellationToken)
    {
        new CreateBranchOfficeRequestValidator().ValidateAndThrow(request);

        var branchOffice = BranchOffice.Create(
            request.CreatedBy,
            request.TenantId,
            request.Name,
            request.Address,
            request.Contacts,
            request.Schedule);

        if (branchOffice.HasChanged)
        {
            await _branchOfficeRepository.CreateAsync(branchOffice);
            await _eventProcessor.ProcessAsync(branchOffice.Events);
        }

        return branchOffice.Id;
    }
}
