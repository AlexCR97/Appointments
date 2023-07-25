namespace Appointments.Infrastructure.Services.Geo;

public interface IGeoServiceOptions
{
    string ApiUrl { get; }
    string Format { get; }
}

internal sealed record GeoServiceOptions(
    string ApiUrl,
    string Format) : IGeoServiceOptions;
