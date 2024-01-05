using Appointments.Common.Domain.Models;

namespace Appointments.Infrastructure.Mongo.Documents;

internal sealed record CoordinatesDocument(
    int Longitude,
    int Latitude)
{
    internal static CoordinatesDocument From(Coordinates coordinates)
    {
        return new CoordinatesDocument(
            coordinates.Longitude,
            coordinates.Latitude);
    }

    internal Coordinates ToEntity()
    {
        return new Coordinates(
            Longitude,
            Latitude);
    }
}
