using Appointments.Common.Domain.Models;

namespace Appointments.Api.Tenant.Models;

public sealed record CoordinatesModel(
    int Longitude,
    int Latitude)
{
    internal static CoordinatesModel From(Coordinates coordinates)
    {
        return new CoordinatesModel(coordinates.Longitude, coordinates.Latitude);
    }

    internal Coordinates ToModel()
    {
        return new Coordinates(Longitude, Latitude);
    }
}
