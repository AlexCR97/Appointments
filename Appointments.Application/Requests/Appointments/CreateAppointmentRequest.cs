using Appointments.Common.Domain;
using Appointments.Core.Domain.Entities;
using FluentValidation;
using MediatR;

namespace Appointments.Core.Application.Requests.Appointments;

public sealed record CreateAppointmentRequest(
    string CreatedBy,
    Guid TenantId,
    Guid BranchOfficeId,
    Guid ServiceId,
    Guid? ServiceProviderId,
    DateTime AppointmentDate,
    AppointmentCustomer Customer,
    string? Notes)
    : IRequest<Guid>;

internal sealed class CreateAppointmentRequestValidator : AbstractValidator<CreateAppointmentRequest>
{
    public CreateAppointmentRequestValidator()
    {
        RuleFor(x => x.CreatedBy)
            .NotEmpty();

        RuleFor(x => x.TenantId)
            .NotEmpty();

        RuleFor(x => x.BranchOfficeId)
            .NotEmpty();

        RuleFor(x => x.ServiceId)
            .NotEmpty();

        When(x => x.ServiceProviderId is not null, () =>
        {
            RuleFor(x => x.ServiceProviderId!.Value)
                .NotEmpty();
        });

        RuleFor(x => x.AppointmentDate)
            .NotEmpty();

        RuleFor(x => x.Customer)
            .SetValidator(new AppointmentCustomerValidator());
    }
}

internal sealed class CreateAppointmentRequestHandler : IRequestHandler<CreateAppointmentRequest, Guid>
{
    private readonly IEventProcessor _eventProcessor;
    private readonly IAppointmentRepository _appointmentRepository;

    public CreateAppointmentRequestHandler(IEventProcessor eventProcessor, IAppointmentRepository appointmentRepository)
    {
        _eventProcessor = eventProcessor;
        _appointmentRepository = appointmentRepository;
    }

    public async Task<Guid> Handle(CreateAppointmentRequest request, CancellationToken cancellationToken)
    {
        new CreateAppointmentRequestValidator().ValidateAndThrow(request);

        var appointment = Appointment.Create(
            request.CreatedBy,
            request.TenantId,
            request.BranchOfficeId,
            request.ServiceId,
            request.ServiceProviderId,
            request.AppointmentDate,
            AppointmentCode.Random(),
            request.Customer,
            request.Notes,
            AppointmentStatus.Queued);

        if (appointment.HasChanged)
        {
            await _appointmentRepository.CreateAsync(appointment);
            await _eventProcessor.ProcessAsync(appointment.Events);
        }

        return appointment.Id;
    }
}
