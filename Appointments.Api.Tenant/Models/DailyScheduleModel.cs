using Appointments.Common.Domain.Models;
using Appointments.Core.Domain.Entities;

namespace Appointments.Api.Tenant.Models;

public sealed record DailyScheduleModel(
    DateRangeModel[]? Hours,
    bool Disabled)
{
    internal static DailyScheduleModel From(DailySchedule schedule)
    {
        return new DailyScheduleModel(
            schedule.Hours
                .Select(DateRangeModel.From)
                .ToArray(),
            schedule.Disabled);
    }

    internal DailySchedule ToModel()
    {
        return new DailySchedule(
            Hours
                ?.Select(x => x.ToModel())
                .ToList()
                ?? new List<DateRange>(),
            Disabled);
    }
}
