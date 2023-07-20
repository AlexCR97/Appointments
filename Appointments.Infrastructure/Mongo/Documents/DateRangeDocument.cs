namespace Appointments.Infrastructure.Mongo.Documents;

internal class DateRangeDocument
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool Disabled { get; set; }
}
