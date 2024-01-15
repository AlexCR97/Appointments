using Appointments.Common.Domain.Models;

namespace Appointments.Api.Tenant.Models;

public sealed record DateRangeModel(
    DateTime StartDate,
    DateTime EndDate,
    bool Disabled)
{
    internal static DateRangeModel From(DateRange range)
    {
        return new DateRangeModel(
            range.StartDate,
            range.EndDate,
            range.Disabled);
    }

    internal DateRange ToModel()
    {
        return new DateRange(
            StartDate,
            EndDate,
            Disabled);
    }
}
