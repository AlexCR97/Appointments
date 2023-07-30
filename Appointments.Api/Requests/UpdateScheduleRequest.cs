using Appointments.Domain.Entities;

namespace Appointments.Api.Requests;

public sealed record UpdateScheduleRequest(
    Guid Id,
    WeeklySchedule WeeklySchedule);
