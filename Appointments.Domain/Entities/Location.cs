namespace Appointments.Domain.Entities;

public class Location
{
    public double Lat { get; }
    public double Lng { get; }

    public Location(double lat, double lng)
    {
        Lat = lat;
        Lng = lng;
    }

    public static Location Empty
        => new(0, 0);
}
