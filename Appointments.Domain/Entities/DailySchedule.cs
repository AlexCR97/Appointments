using Appointments.Domain.Models;
using FluentValidation;

namespace Appointments.Domain.Entities;

public sealed record DailySchedule(
    List<DateRange> Hours,
    bool Disabled)
{
    public static DailySchedule Weekday()
    {
        return new DailySchedule(
            new List<DateRange> { DateRange.NineToFive() },
            false);
    }

    public static DailySchedule Weekend()
    {
        return new DailySchedule(
            new List<DateRange>(),
            false);
    }
}

public sealed class DailyScheduleValidator : AbstractValidator<DailySchedule>
{
    public DailyScheduleValidator()
    {
        RuleForEach(x => x.Hours)
            .SetValidator(new DateRangeValidator());
    }
}
