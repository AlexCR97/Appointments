using Appointments.Core.Domain.Entities;

namespace Appointments.Infrastructure.Mongo.Documents;

internal sealed record DailyScheduleDocument(
    DateRangeDocument[] Hours,
    bool Disabled)
{
    internal static DailyScheduleDocument From(DailySchedule schedule)
    {
        return new DailyScheduleDocument(
            schedule.Hours
                .Select(DateRangeDocument.From)
                .ToArray(),
            schedule.Disabled);
    }

    internal DailySchedule ToModel()
    {
        return new DailySchedule(
            Hours
                .Select(x => x.ToModel())
                .ToList(),
            Disabled);
    }
}
