using Appointments.Api.Extensions.AspNetCore;
using Appointments.Api.Extensions.Files;
using Appointments.Application.Policies;
using Appointments.Application.Requests.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Appointments.Api.Controllers;

[Route("api/services")]
[ApiController]
[Authorize(Policy = ServicePolicy.PolicyName)]
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

    [HttpPost("{serviceId}/images")]
    [Authorize(Roles = $"{ServicePolicy.Roles.Owner},{ServicePolicy.Roles.Admin}")]
    public async Task<IActionResult> UploadServiceImages(
        [FromRoute] Guid serviceId,
        [FromForm] IFormFileCollection images)
    {
        var indexedImages = await _mediator.Send(new UploadImagesRequest(
            User.GetUsername(),
            serviceId,
            images
                .Select((image, index) => new UploadImagesRequest.IndexedImage(
                    index,
                    image.FileName,
                    image.GetBytes()))
                .ToList()));

        return Ok(indexedImages);
    }
}
