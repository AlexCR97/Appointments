namespace Appointments.Domain.Entities;

public class DailySchedule
{
    public bool Disabled { get; set; }
    public List<DateRange> Hours { get; set; }
}
