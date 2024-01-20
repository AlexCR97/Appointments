using RestSharp;

namespace Appointments.Core.Infrastructure.Services.Geo;

public interface IGeoService
{
    Task<GeoReverseResponse> ReverseAsync(double latitude, double longitude);
    Task<IReadOnlyList<GeoSearchResponse>> SearchAsync(
        string? query = null,
        string? street = null,
        string? city = null,
        string? county = null,
        string? state = null,
        string? country = null,
        int? postalCode = null);
}

internal class GeoService : IGeoService
{
    private readonly IGeoServiceOptions _options;

    public GeoService(IGeoServiceOptions options)
    {
        _options = options;
        _restClient = new RestClient(options.ApiUrl);
    }

    private RestClient _restClient;

    public async Task<GeoReverseResponse> ReverseAsync(double latitude, double longitude)
    {
        var restRequest = new RestRequest("reverse", Method.Get);
        restRequest.AddQueryParameter("format", _options.Format);
        restRequest.AddQueryParameter("lat", latitude);
        restRequest.AddQueryParameter("lon", longitude);

        var response = await _restClient.ExecuteAsync<GeoReverseResponse>(restRequest);

        if (!response.IsSuccessful)
            throw new HttpRequestException("HTTP request failed");

        if (response.Data is null)
            throw new HttpRequestException("HTTP response did not contain any data");

        return response.Data;
    }

    public async Task<IReadOnlyList<GeoSearchResponse>> SearchAsync(string? query = null, string? street = null, string? city = null, string? county = null, string? state = null, string? country = null, int? postalCode = null)
    {
        var restRequest = new RestRequest("search", Method.Get);
        restRequest.AddQueryParameter("format", _options.Format);
        restRequest.AddQueryParameter("q", query);
        restRequest.AddQueryParameter("street", street);
        restRequest.AddQueryParameter("city", city);
        restRequest.AddQueryParameter("county", county);
        restRequest.AddQueryParameter("country", country);
        restRequest.AddQueryParameter("state", state);

        var response = await _restClient.ExecuteAsync<IReadOnlyList<GeoSearchResponse>>(restRequest);

        if (!response.IsSuccessful)
            throw new HttpRequestException("HTTP request failed");

        if (response.Data is null)
            throw new HttpRequestException("HTTP response did not contain any data");

        return response.Data;
    }
}
