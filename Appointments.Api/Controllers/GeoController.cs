using Appointments.Infrastructure.Services.Geo;
using Microsoft.AspNetCore.Mvc;

namespace Appointments.Api.Controllers;

[Route("api/geo")]
[ApiController]
public class GeoController : ControllerBase
{
    private readonly IGeoService _geoService;

    public GeoController(IGeoService geoService)
    {
        _geoService = geoService;
    }

    [HttpGet("reverse", Name = nameof(Reverse))]
    public async Task<IActionResult> Reverse(
        [FromQuery] double latitude,
        [FromQuery] double longitude)
    {
        var response = await _geoService.ReverseAsync(latitude, longitude);
        return Ok(response);
    }

    [HttpGet("search", Name = nameof(Search))]
    public async Task<IActionResult> Search(
        [FromQuery] string? query,
        [FromQuery] string? street,
        [FromQuery] string? city,
        [FromQuery] string? county,
        [FromQuery] string? state,
        [FromQuery] string? country,
        [FromQuery] int? postalCode)
    {
        var response = await _geoService.SearchAsync(
            query: query,
            street: street,
            city: city,
            county: county,
            state: state,
            country: country,
            postalCode: postalCode);

        return Ok(response);
    }
}
