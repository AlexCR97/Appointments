using Appointments.Application.Requests.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Appointments.Api.Controllers;

[Route("api/services")]
[ApiController]
public class ServicesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ServicesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet(Name = nameof(GetServices))]
    public async Task<IActionResult> GetServices(
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? sort = null,
        [FromQuery] string? filter = null)
    {
        var services = await _mediator.Send(new GetServicesRequest(
            pageIndex,
            pageSize,
            sort,
            filter));

        return Ok(services);
    }
}
