namespace Appointments.Domain.Entities;

public class WeeklySchedule
{
    public DailySchedule Monday { get; set; }
    public DailySchedule Tuesday { get; set; }
    public DailySchedule Wednesday { get; set; }
    public DailySchedule Thursday { get; set; }
    public DailySchedule Friday { get; set; }
    public DailySchedule Saturday { get; set; }
    public DailySchedule Sunday { get; set; }
}
