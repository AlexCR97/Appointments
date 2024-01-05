using Appointments.Common.Application;
using Appointments.Common.Domain.Models;
using Appointments.Core.Domain.Entities;
using FluentValidation;
using MediatR;

namespace Appointments.Core.Application.Requests.Appointments;

public sealed record FindAppointmentsRequest : FindRequest<Appointment>
{
    public FindAppointmentsRequest(int PageIndex, int PageSize, string? Sort, string? Filter) : base(PageIndex, PageSize, Sort, Filter)
    {
    }
}

internal sealed class FindAppointmentsRequestHandler : IRequestHandler<FindAppointmentsRequest, PagedResult<Appointment>>
{
    private readonly IAppointmentRepository _appointmentRepository;

    public FindAppointmentsRequestHandler(IAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }

    public async Task<PagedResult<Appointment>> Handle(FindAppointmentsRequest request, CancellationToken cancellationToken)
    {
        new FindRequestValidator<Appointment>().ValidateAndThrow(request);

        return await _appointmentRepository.FindAsync(
            request.PageIndex,
            request.PageSize,
            request.Sort,
            request.Filter);
    }
}
