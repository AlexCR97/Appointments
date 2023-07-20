namespace Appointments.Infrastructure.Mongo.Documents;

internal class WeeklyScheduleDocument
{
    public DailyScheduleDocument Monday { get; set; }
    public DailyScheduleDocument Tuesday { get; set; }
    public DailyScheduleDocument Wednesday { get; set; }
    public DailyScheduleDocument Thursday { get; set; }
    public DailyScheduleDocument Friday { get; set; }
    public DailyScheduleDocument Saturday { get; set; }
    public DailyScheduleDocument Sunday { get; set; }
}
