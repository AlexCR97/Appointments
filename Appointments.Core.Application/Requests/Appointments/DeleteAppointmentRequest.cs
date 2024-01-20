using Appointments.Common.Application;
using Appointments.Common.Domain;
using FluentValidation;
using MediatR;

namespace Appointments.Core.Application.Requests.Appointments;

public sealed record DeleteAppointmentRequest : DeleteRequest
{
    public DeleteAppointmentRequest(string DeletedBy, Guid Id) : base(DeletedBy, Id)
    {
    }
}

internal sealed class DeleteAppointmentRequestHandler : IRequestHandler<DeleteAppointmentRequest>
{
    private readonly IEventProcessor _eventProcessor;
    private readonly IAppointmentRepository _appointmentRepository;

    public DeleteAppointmentRequestHandler(IEventProcessor eventProcessor, IAppointmentRepository appointmentRepository)
    {
        _eventProcessor = eventProcessor;
        _appointmentRepository = appointmentRepository;
    }

    public async Task Handle(DeleteAppointmentRequest request, CancellationToken cancellationToken)
    {
        new DeleteRequestValidator().ValidateAndThrow(request);

        var appointment = await _appointmentRepository.GetAsync(request.Id);

        await _appointmentRepository.DeleteAsync(appointment.Id);
        await _eventProcessor.ProcessAsync(appointment.Events);
    }
}
