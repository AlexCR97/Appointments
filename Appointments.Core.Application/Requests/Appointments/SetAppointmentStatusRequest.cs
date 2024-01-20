using Appointments.Common.Domain;
using Appointments.Core.Domain.Entities;
using FluentValidation;
using MediatR;

namespace Appointments.Core.Application.Requests.Appointments;

public sealed record SetAppointmentStatusRequest(
    string UpdatedBy,
    Guid Id,
    AppointmentStatus Status)
    : IRequest;

internal sealed class SetAppointmentStatusRequestValidator : AbstractValidator<SetAppointmentStatusRequest>
{
    public SetAppointmentStatusRequestValidator()
    {
        RuleFor(x => x.UpdatedBy)
            .NotEmpty();

        RuleFor(x => x.Id)
            .NotEmpty();
    }
}

internal class SetAppointmentStatusRequestHandler : IRequestHandler<SetAppointmentStatusRequest>
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IEventProcessor _eventProcessor;

    public SetAppointmentStatusRequestHandler(IAppointmentRepository appointmentRepository, IEventProcessor eventProcessor)
    {
        _appointmentRepository = appointmentRepository;
        _eventProcessor = eventProcessor;
    }

    public async Task Handle(SetAppointmentStatusRequest request, CancellationToken cancellationToken)
    {
        new SetAppointmentStatusRequestValidator().ValidateAndThrow(request);

        var appointment = await _appointmentRepository.GetAsync(request.Id);

        appointment.SetStatus(
            request.UpdatedBy,
            request.Status);

        if (appointment.HasChanged)
        {
            await _appointmentRepository.UpdateAsync(appointment);
            await _eventProcessor.ProcessAsync(appointment.Events);
        }
    }
}
