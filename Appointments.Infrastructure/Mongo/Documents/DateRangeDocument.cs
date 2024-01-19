using Appointments.Common.Domain.Models;

namespace Appointments.Infrastructure.Mongo.Documents;

internal sealed record DateRangeDocument(
    DateTime StartDate,
    DateTime EndDate,
    bool Disabled)
{
    internal static DateRangeDocument From(DateRange dateRange)
    {
        return new DateRangeDocument(
            dateRange.StartDate,
            dateRange.EndDate,
            dateRange.Disabled);
    }

    internal DateRange ToModel()
    {
        return new DateRange(
            StartDate,
            EndDate,
            Disabled);
    }
}
