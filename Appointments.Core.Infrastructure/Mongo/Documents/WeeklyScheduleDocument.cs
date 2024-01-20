using Appointments.Core.Domain.Entities;

namespace Appointments.Infrastructure.Mongo.Documents;

internal sealed record WeeklyScheduleDocument(
    DailyScheduleDocument Monday,
    DailyScheduleDocument Tuesday,
    DailyScheduleDocument Wednesday,
    DailyScheduleDocument Thursday,
    DailyScheduleDocument Friday,
    DailyScheduleDocument Saturday,
    DailyScheduleDocument Sunday)
{
    internal static WeeklyScheduleDocument From(WeeklySchedule schedule)
    {
        return new WeeklyScheduleDocument(
            DailyScheduleDocument.From(schedule.Monday),
            DailyScheduleDocument.From(schedule.Tuesday),
            DailyScheduleDocument.From(schedule.Wednesday),
            DailyScheduleDocument.From(schedule.Thursday),
            DailyScheduleDocument.From(schedule.Friday),
            DailyScheduleDocument.From(schedule.Saturday),
            DailyScheduleDocument.From(schedule.Sunday));
    }

    internal WeeklySchedule ToEntity()
    {
        return new WeeklySchedule(
            Monday.ToModel(),
            Tuesday.ToModel(),
            Wednesday.ToModel(),
            Thursday.ToModel(),
            Friday.ToModel(),
            Saturday.ToModel(),
            Sunday.ToModel());
    }
}
