namespace Appointments.Infrastructure.Mongo.Documents;

internal class DailyScheduleDocument
{
    public bool Disabled { get; set; }
    public List<DateRangeDocument> Hours { get; set; } = null!;
}
