using FluentValidation;

namespace Appointments.Domain.Entities;

public sealed record WeeklySchedule(
    DailySchedule Monday,
    DailySchedule Tuesday,
    DailySchedule Wednesday,
    DailySchedule Thursday,
    DailySchedule Friday,
    DailySchedule Saturday,
    DailySchedule Sunday)
{
    public static WeeklySchedule Default()
    {
        return new WeeklySchedule(
            DailySchedule.Weekday(),
            DailySchedule.Weekday(),
            DailySchedule.Weekday(),
            DailySchedule.Weekday(),
            DailySchedule.Weekday(),
            DailySchedule.Weekend(),
            DailySchedule.Weekend());
    }
}

public sealed class WeeklyScheduleValidator : AbstractValidator<WeeklySchedule>
{
    public WeeklyScheduleValidator()
    {
        RuleFor(x => x.Monday).SetValidator(new DailyScheduleValidator());
        RuleFor(x => x.Tuesday).SetValidator(new DailyScheduleValidator());
        RuleFor(x => x.Wednesday).SetValidator(new DailyScheduleValidator());
        RuleFor(x => x.Thursday).SetValidator(new DailyScheduleValidator());
        RuleFor(x => x.Friday).SetValidator(new DailyScheduleValidator());
        RuleFor(x => x.Saturday).SetValidator(new DailyScheduleValidator());
        RuleFor(x => x.Sunday).SetValidator(new DailyScheduleValidator());
    }
}
