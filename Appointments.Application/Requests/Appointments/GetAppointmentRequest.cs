using Appointments.Common.Application;
using Appointments.Core.Domain.Entities;
using FluentValidation;
using MediatR;

namespace Appointments.Core.Application.Requests.Appointments;

public sealed record GetAppointmentRequest : GetRequest<Appointment>
{
    public GetAppointmentRequest(Guid Id) : base(Id)
    {
    }
}

internal sealed class GetAppointmentRequestHandler : IRequestHandler<GetAppointmentRequest, Appointment>
{
    private readonly IAppointmentRepository _appointmentRepository;

    public GetAppointmentRequestHandler(IAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }

    public async Task<Appointment> Handle(GetAppointmentRequest request, CancellationToken cancellationToken)
    {
        new GetRequestValidator<Appointment>().ValidateAndThrow(request);

        return await _appointmentRepository.GetAsync(request.Id);
    }
}
