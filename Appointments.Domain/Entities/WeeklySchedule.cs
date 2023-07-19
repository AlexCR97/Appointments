namespace Appointments.Domain.Entities;

public class WeeklySchedule
{
    public DailySchedule Monday { get; }
    public DailySchedule Tuesday { get; }
    public DailySchedule Wednesday { get; }
    public DailySchedule Thursday { get; }
    public DailySchedule Friday { get; }
    public DailySchedule Saturday { get; }
    public DailySchedule Sunday { get; }

    public WeeklySchedule(DailySchedule monday, DailySchedule tuesday, DailySchedule wednesday, DailySchedule thursday, DailySchedule friday, DailySchedule saturday, DailySchedule sunday)
    {
        Monday = monday;
        Tuesday = tuesday;
        Wednesday = wednesday;
        Thursday = thursday;
        Friday = friday;
        Saturday = saturday;
        Sunday = sunday;
    }

    public static WeeklySchedule NineToFive => new(
        DailySchedule.NineToFive,
        DailySchedule.NineToFive,
        DailySchedule.NineToFive,
        DailySchedule.NineToFive,
        DailySchedule.NineToFive,
        new DailySchedule(true, null),
        new DailySchedule(true, null));
}
