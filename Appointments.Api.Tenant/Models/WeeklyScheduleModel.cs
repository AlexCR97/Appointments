using Appointments.Core.Domain.Entities;

namespace Appointments.Api.Tenant.Models;

public sealed record WeeklyScheduleModel(
    DailyScheduleModel Monday,
    DailyScheduleModel Tuesday,
    DailyScheduleModel Wednesday,
    DailyScheduleModel Thursday,
    DailyScheduleModel Friday,
    DailyScheduleModel Saturday,
    DailyScheduleModel Sunday)
{
    internal static WeeklyScheduleModel From(WeeklySchedule schedule)
    {
        return new WeeklyScheduleModel(
            DailyScheduleModel.From(schedule.Monday),
            DailyScheduleModel.From(schedule.Tuesday),
            DailyScheduleModel.From(schedule.Wednesday),
            DailyScheduleModel.From(schedule.Thursday),
            DailyScheduleModel.From(schedule.Friday),
            DailyScheduleModel.From(schedule.Saturday),
            DailyScheduleModel.From(schedule.Sunday));
    }

    internal static WeeklyScheduleModel? FromOrDefault(WeeklySchedule? schedule)
    {
        if (schedule is null)
            return null;

        return From(schedule);
    }

    internal WeeklySchedule ToModel()
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
