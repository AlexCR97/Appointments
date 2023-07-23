using Appointments.Api.Extensions.Files;
using Appointments.Application.Requests.Tenants;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Appointments.Api.Controllers;

[Route("api/tenants")]
[ApiController]
public class TenantsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TenantsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{tenantId}", Name = nameof(GetTenant))]
    public async Task<IActionResult> GetTenant([FromRoute] Guid tenantId)
    {
        var tenant = await _mediator.Send(new GetTenantRequest(tenantId));
        return Ok(tenant);
    }

    [HttpPost("{tenantId}/logo", Name = nameof(UploadTenantLogo))]
    public async Task<IActionResult> UploadTenantLogo(
        [FromRoute] Guid tenantId,
        [FromForm] IFormFile image)
    {
        var tenantLogoPath = await _mediator.Send(new UploadLogoRequest(
            tenantId,
            image.FileName,
            image.GetBytes(),
            null // TODO Set UpdatedBy
            ));

        return Ok(new { logoPath = tenantLogoPath });
    }
}
