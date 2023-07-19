namespace Appointments.Domain.Entities;

public class DailySchedule
{
    public bool Disabled { get; }
    public List<DateRange> Hours { get; }

    public DailySchedule(bool disabled, List<DateRange>? hours)
    {
        Disabled = disabled;
        Hours = hours ?? new();
    }

    public static DailySchedule NineToFive => new(
        false,
        new List<DateRange>
        {
            new DateRange(
                new DateTime().AddHours(9),
                new DateTime().AddHours(17),
                false),
        });
}
