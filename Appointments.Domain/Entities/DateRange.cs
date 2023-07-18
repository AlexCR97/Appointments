namespace Appointments.Domain.Entities;

public class DateRange
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool Disabled { get; set; }
}
