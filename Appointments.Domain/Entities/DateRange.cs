namespace Appointments.Domain.Entities;

public class DateRange
{
    public DateTime StartDate { get; }
    public DateTime EndDate { get; }
    public bool Disabled { get; }

    public DateRange(DateTime startDate, DateTime endDate, bool disabled)
    {
        StartDate = startDate;
        EndDate = endDate;
        Disabled = disabled;
    }
}
