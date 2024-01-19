using FluentValidation;

namespace Appointments.Common.Domain.Models;

public readonly struct Coordinates
{
    public int Longitude { get; }
    public int Latitude { get; }

    public Coordinates(int longitude, int latitude)
    {
        Longitude = longitude;
        Latitude = latitude;
        new CoordinatesValidator().ValidateAndThrow(this);
    }

    public override string ToString()
    {
        return $"{Longitude}, {Latitude}";
    }

    public static Coordinates Default()
    {
        return new Coordinates();
    }
}

public sealed class CoordinatesValidator : AbstractValidator<Coordinates>
{
    public CoordinatesValidator()
    {
    }
}
