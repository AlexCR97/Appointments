using Appointments.Application.Requests.Services;
using Appointments.Domain.Models;
using Appointments.Domain.Models.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Appointments.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ServicesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ServicesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet(Name = nameof(GetServices))]
    public async Task<PagedResult<ServiceModel>> GetServices(
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? sort = null,
        [FromQuery] string? filter = null)
    {
        return await _mediator.Send(new GetServicesRequest(
            pageIndex,
            pageSize,
            sort,
            filter));
    }
}
